/*
 * SimpleObjectPool.cs
 * Chase Kurkowski
 * Scott Duman
 * Script that lets us pool and easily access objects.
 */
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class SimpleObjectPool
{
	///<summary>
	///The base size of the pool.
	///</summary>
	const int DEFAULT_POOL_SIZE = 5;

	///<summary>
	///The class that holds the individual prefabs and inactive items.
	///</summary>
	private class Pool
	{
		///<summary>
		///A cosmetic ID number for the prefabs.
		///</summary>
		int nextID = 1;

		readonly Stack<GameObject> inactiveObjects;

		readonly GameObject prefab;

		///<summary>
		///Constructor for the Pool class.
		///</summary>
		public Pool(GameObject prefab, int initialQuantity) {
			this.prefab = prefab;
			inactiveObjects = new Stack<GameObject>(initialQuantity);
		}

		///<summary>
		///Checks for inactive objects, then spawns a new one if there aren't any.
		///</summary>
		public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent = null) {
			GameObject spawnedObject;
			bool initalSpawn = false;
			if(inactiveObjects.Count == 0) {
				spawnedObject = GameObject.Instantiate(prefab, pos, rot);
				spawnedObject.name = prefab.name + $"({nextID++})";

				PoolMember poolMember = spawnedObject.AddComponent<PoolMember>();
				poolMember.pool = this;

				spawnedObject.BroadcastMessage("SetupSpawnable", poolMember, SendMessageOptions.DontRequireReceiver);
				initalSpawn = true;
			} else {
				spawnedObject = inactiveObjects.Pop();
				if (spawnedObject == null) {
					return Spawn(pos, rot, parent);
				}
			}

			spawnedObject.transform.position = pos;
			spawnedObject.transform.rotation = rot;
			if (parent != null) {
				spawnedObject.transform.SetParent(parent);
			}
			spawnedObject.SetActive(true);
			spawnedObject.GetComponent<PoolMember>().TriggerOnSpawn(initalSpawn);
			return spawnedObject;
		}

		///<summary>
		///Deactivates the object and adds it back into the stack.
		///</summary>
		public void Despawn(GameObject objToDespawn)
		{
			objToDespawn.SetActive(false);

			inactiveObjects.Push(objToDespawn);
		}
	}

	///<summary>
	///Class that is added to spawned objects. Holds a reference to the Pool they were spawned from.
	///</summary>
	private class PoolMember : IPoolEvents {
		public Pool pool;

		public void TriggerOnSpawn(bool initalSpawn = false) => InvokeOnSpawn(initalSpawn);
	}

	// public pool events interface so other components can listen for pool events
	public abstract class IPoolEvents : MonoBehaviour {
		public event System.EventHandler<bool> OnSpawn;
		protected void InvokeOnSpawn(bool inital) => OnSpawn?.Invoke(gameObject, inital);
	}

	///<summary>
	///A dictionary to hold all of the different pools.
	///</summary>
	static Dictionary< GameObject, Pool > pools;
	// TODO: Clear Dictionary pool when scene changes

	///<summary>
	///Function that initializes the Dictionary with new Pools if one does not already exist.
	///</summary>
	static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE) {
		if(pools == null) {
			pools = new Dictionary<GameObject, Pool>();
		}
		
		if(prefab != null && pools.ContainsKey(prefab) == false) {
			pools[prefab] = new Pool(prefab, qty);
		}
	}

	///<summary>
	///Function that allows for preloading of objects. This can be helful with objects that you know you will need lots of in quick succession. Not necessary for most items/objects.
	///</summary>
	public static void Preload(GameObject prefab, int qty = 1) {
		Init(prefab, qty);

		GameObject[] objects = new GameObject[qty];
		for(int i = 0; i < qty; i++) {
			objects[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
		}

		for(int i = 0; i < qty; i++) {
			Despawn(objects[i]);
		}
	}

    public static GameObject Spawn(GameObject prefab, Vector3 pos) {
        return Spawn(prefab, pos, Quaternion.identity);
    }

    public static GameObject Spawn(GameObject prefab, Vector3 pos, Vector3 rot, Transform parent = null) {
        return Spawn(prefab, pos, Quaternion.Euler(rot), parent);
    }

	///<summary>
	///Function that initializes a new Pool (if not already there) and then spawns a prefab from it.
	///</summary>
	public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null) {
		Init(prefab);
		return pools[prefab].Spawn(pos, rot, parent);
	}

	///<summary>
	///Checks that the object is a poolmember and then Despawns it, if it is not a poolmember it destroys it.
	///</summary>
	public static void Despawn(GameObject objToDespawn) {
		PoolMember poolMember = objToDespawn.GetComponent<PoolMember>();
		if(poolMember == null) {
			//Debug.LogWarning("Object " + objToDespawn.name + " was not spawned from a pool. Destroying it.");
			GameObject.Destroy(objToDespawn);
		} else {
			poolMember.pool.Despawn(objToDespawn);
		}
	}

	private static void Despawn(PoolMember poolMember) {
		if (poolMember != null) {
			poolMember.pool.Despawn(poolMember.gameObject);
		} else {
			Debug.LogWarning("WARNING: Given pool member does not exist!");
		}
	}

	///<summary>
	/// Delays the despawn of any game object based on the given delay time which is defaults to 1 second.
	///</summary>
	public static void DealyDespawn(GameObject objToDespawn, float delay = 1f) {
		PoolMember poolMember = objToDespawn.GetComponent<PoolMember>();
		if (poolMember == null) {
			Debug.LogWarning("WARNING: Object " + objToDespawn.name + " was not spawned from a pool. Destroying it.");
		} else {
			poolMember.StartCoroutine(DelayedDespawn(poolMember, delay));
		}
	}

	private static IEnumerator DelayedDespawn(PoolMember poolMember, float delay)
	{
		//Debug.Log("Running Delayed Despawn");
		yield return new WaitForSeconds(delay);
		Despawn(poolMember);
	}
}
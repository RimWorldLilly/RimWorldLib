using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002330 RID: 9008
	public static class WorldTerrainColliderManager
	{
		// Token: 0x0600DE89 RID: 56969 RVA: 0x0052C3A8 File Offset: 0x0052A5A8
		public static void ClearCache()
		{
			foreach (KeyValuePair<PlanetLayer, Dictionary<int, GameObject>> keyValuePair in WorldTerrainColliderManager.layerWorldTerrainColliders)
			{
				PlanetLayer planetLayer;
				Dictionary<int, GameObject> dictionary;
				keyValuePair.Deconstruct(out planetLayer, out dictionary);
				foreach (KeyValuePair<int, GameObject> keyValuePair2 in dictionary)
				{
					int num;
					GameObject obj;
					keyValuePair2.Deconstruct(out num, out obj);
					Object.Destroy(obj);
				}
			}
			WorldTerrainColliderManager.layerWorldTerrainColliders.Clear();
		}

		// Token: 0x0600DE8A RID: 56970 RVA: 0x0052C454 File Offset: 0x0052A654
		public static void EnsureRaycastCollidersUpdated()
		{
			foreach (KeyValuePair<PlanetLayer, Dictionary<int, GameObject>> keyValuePair in WorldTerrainColliderManager.layerWorldTerrainColliders)
			{
				PlanetLayer planetLayer;
				Dictionary<int, GameObject> dictionary;
				keyValuePair.Deconstruct(out planetLayer, out dictionary);
				PlanetLayer planetLayer2 = planetLayer;
				foreach (KeyValuePair<int, GameObject> keyValuePair2 in dictionary)
				{
					int num;
					GameObject gameObject;
					keyValuePair2.Deconstruct(out num, out gameObject);
					gameObject.SetActive(planetLayer2.Raycastable);
				}
			}
		}

		// Token: 0x0600DE8B RID: 56971 RVA: 0x0052C500 File Offset: 0x0052A700
		private static GameObject CreateGameObject(PlanetLayer planetLayer, int layer)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
			defaultInterpolatedStringHandler.AppendFormatted<PlanetLayer>(planetLayer);
			defaultInterpolatedStringHandler.AppendLiteral(" WorldTerrainCollider layer ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(layer);
			GameObject gameObject = new GameObject(defaultInterpolatedStringHandler.ToStringAndClear());
			Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = layer;
			return gameObject;
		}

		// Token: 0x0600DE8C RID: 56972 RVA: 0x0052C54C File Offset: 0x0052A74C
		public static GameObject Get(PlanetLayer planetLayer, int layer)
		{
			Dictionary<int, GameObject> dictionary;
			if (!WorldTerrainColliderManager.layerWorldTerrainColliders.TryGetValue(planetLayer, out dictionary))
			{
				dictionary = (WorldTerrainColliderManager.layerWorldTerrainColliders[planetLayer] = new Dictionary<int, GameObject>());
			}
			GameObject gameObject;
			if (!dictionary.TryGetValue(layer, out gameObject))
			{
				gameObject = (dictionary[layer] = WorldTerrainColliderManager.CreateGameObject(planetLayer, layer));
			}
			gameObject.SetActive(false);
			return gameObject;
		}

		// Token: 0x04008EE7 RID: 36583
		private static readonly Dictionary<PlanetLayer, Dictionary<int, GameObject>> layerWorldTerrainColliders = new Dictionary<PlanetLayer, Dictionary<int, GameObject>>();
	}
}

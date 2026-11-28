using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000708 RID: 1800
	public static class MatLoader
	{
		// Token: 0x060038F6 RID: 14582 RVA: 0x0016DA0E File Offset: 0x0016BC0E
		public static void ClearCache()
		{
			MatLoader.dict.Clear();
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x0016DA1C File Offset: 0x0016BC1C
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material;
			if (!MatLoader.dict.TryGetValue(key, out material))
			{
				Material material2 = MatLoader.TryLoadMaterialDef(matPath, -1, false);
				if (material2 == null)
				{
					Log.Warning("Could not load material " + matPath);
					return BaseContent.BadMat;
				}
				material = MaterialAllocator.Create(material2);
				material.name = "MatLoader.LoadMat";
				if (renderQueue != -1)
				{
					material.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material);
			}
			return material;
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0016DAA4 File Offset: 0x0016BCA4
		public static Material LoadMatDirect(string matPath)
		{
			return MatLoader.TryLoadMaterialDef(matPath, -1, false) ?? BaseContent.BadMat;
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x0016DAB8 File Offset: 0x0016BCB8
		private static Material TryLoadMaterialDef(string matPath, int renderQueue, bool copy)
		{
			MaterialDef materialDef;
			if (MaterialDefUtility.TryGetByResourcePath(matPath, out materialDef))
			{
				if (copy || renderQueue != -1)
				{
					return materialDef.CreateMaterialInstance(renderQueue);
				}
				return materialDef.Material;
			}
			else
			{
				if (DefDatabase<MaterialDef>.DefCount == 0)
				{
					Material material = Material.CreateDeferred(() => MatLoader.ResolveDeferredMaterial(matPath));
					if (renderQueue != -1)
					{
						material.renderQueue = renderQueue;
					}
					return material;
				}
				return null;
			}
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0016DB20 File Offset: 0x0016BD20
		private static Material ResolveDeferredMaterial(string matPath)
		{
			MaterialDef materialDef;
			if (MaterialDefUtility.TryGetByResourcePath(matPath, out materialDef))
			{
				return materialDef.Material;
			}
			if (DefDatabase<MaterialDef>.DefCount == 0)
			{
				return null;
			}
			Log.Error("No MaterialDef for material path '" + matPath + "'.");
			return BaseContent.BadMat;
		}

		// Token: 0x04002517 RID: 9495
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x02002A6F RID: 10863
		private struct Request
		{
			// Token: 0x06010661 RID: 67169 RVA: 0x005C688F File Offset: 0x005C4A8F
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<string>(0, this.path), this.renderQueue);
			}

			// Token: 0x06010662 RID: 67170 RVA: 0x005C68A8 File Offset: 0x005C4AA8
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06010663 RID: 67171 RVA: 0x005C68C0 File Offset: 0x005C4AC0
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06010664 RID: 67172 RVA: 0x005C68E5 File Offset: 0x005C4AE5
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06010665 RID: 67173 RVA: 0x005C68EF File Offset: 0x005C4AEF
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06010666 RID: 67174 RVA: 0x005C68FB File Offset: 0x005C4AFB
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue.ToString(),
					")"
				});
			}

			// Token: 0x0400ADE9 RID: 44521
			public string path;

			// Token: 0x0400ADEA RID: 44522
			public int renderQueue;
		}
	}
}

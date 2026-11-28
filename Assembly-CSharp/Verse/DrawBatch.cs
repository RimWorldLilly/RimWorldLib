using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x0200025D RID: 605
	public class DrawBatch
	{
		// Token: 0x060011A3 RID: 4515 RVA: 0x00065514 File Offset: 0x00063714
		public DrawBatchPropertyBlock GetPropertyBlock()
		{
			DrawBatchPropertyBlock drawBatchPropertyBlock;
			if (this.propertyBlockCache.Count == 0)
			{
				drawBatchPropertyBlock = new DrawBatchPropertyBlock();
				this.myPropertyBlocks.Add(drawBatchPropertyBlock);
			}
			else
			{
				drawBatchPropertyBlock = this.propertyBlockCache.Pop<DrawBatchPropertyBlock>();
			}
			if (DrawBatch.PropertyBlockLeakDebug)
			{
				drawBatchPropertyBlock.leakDebugString = "Allocated from:\n\n---------------\n\n" + StackTraceUtility.ExtractStackTrace();
			}
			return drawBatchPropertyBlock;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0006556E File Offset: 0x0006376E
		public void ReturnPropertyBlock(DrawBatchPropertyBlock propertyBlock)
		{
			if (this.myPropertyBlocks.Contains(propertyBlock))
			{
				this.propertyBlockCache.Add(propertyBlock);
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0006558A File Offset: 0x0006378A
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Color? color = null, bool renderInstanced = false, DrawBatchPropertyBlock propertyBlock = null)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, propertyBlock)).Add(matrix, color);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000655A7 File Offset: 0x000637A7
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, bool renderInstanced = false)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, null)).Add(matrix);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000655C4 File Offset: 0x000637C4
		public void Flush(bool draw = true)
		{
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			this.tmpPropertyBlocks.Clear();
			this.tmpPropertyBlocks.AddRange(this.propertyBlockCache);
			try
			{
				foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in this.batches)
				{
					DrawBatch.BatchKey key = keyValuePair.Key;
					try
					{
						foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
						{
							DrawBatch.BatchData batchData2 = batchData;
							if (draw)
							{
								this.tmpPropertyBlock.Clear();
								if (key.propertyBlock != null)
								{
									key.propertyBlock.Write(this.tmpPropertyBlock);
								}
								if (key.renderInstanced)
								{
									key.material.enableInstancing = true;
									if (batchData2.hasAnyColors)
									{
										this.tmpPropertyBlock.SetVectorArray("_Color", batchData2.colors);
									}
									Graphics.DrawMeshInstanced(key.mesh, 0, key.material, batchData.matrices, batchData.ptr, this.tmpPropertyBlock, ShadowCastingMode.On, true, key.layer);
								}
								else
								{
									for (int i = 0; i < batchData2.ptr; i++)
									{
										Matrix4x4 matrix = batchData2.matrices[i];
										Vector4 v = batchData2.colors[i];
										if (batchData2.hasAnyColors)
										{
											this.tmpPropertyBlock.SetColor("_Color", v);
										}
										Graphics.DrawMesh(key.mesh, matrix, key.material, key.layer, null, 0, this.tmpPropertyBlock);
									}
								}
							}
							batchData2.Clear();
							this.batchDataListCache.Add(batchData2);
						}
					}
					finally
					{
						if (key.propertyBlock != null && this.myPropertyBlocks.Contains(key.propertyBlock))
						{
							this.tmpPropertyBlocks.Add(key.propertyBlock);
							key.propertyBlock.Clear();
							this.propertyBlockCache.Add(key.propertyBlock);
						}
						this.batchListCache.Add(keyValuePair.Value);
						keyValuePair.Value.Clear();
					}
				}
			}
			finally
			{
				foreach (DrawBatchPropertyBlock drawBatchPropertyBlock in this.myPropertyBlocks)
				{
					if (!this.tmpPropertyBlocks.Contains(drawBatchPropertyBlock))
					{
						Log.Warning("Property block from FleckDrawBatch leaked!" + ((drawBatchPropertyBlock.leakDebugString == null) ? null : ("Leak debug information: \n" + drawBatchPropertyBlock.leakDebugString)));
					}
				}
				HashSet<DrawBatchPropertyBlock> hashSet = this.myPropertyBlocks;
				this.myPropertyBlocks = this.tmpPropertyBlocks;
				this.tmpPropertyBlocks = hashSet;
				this.batches.Clear();
				this.lastBatchKey = default(DrawBatch.BatchKey);
				this.lastBatchList = null;
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00065920 File Offset: 0x00063B20
		private DrawBatch.BatchData GetBatchDataForInsertion(DrawBatch.BatchKey key)
		{
			List<DrawBatch.BatchData> list;
			if (this.lastBatchList != null && key.GetHashCode() == this.lastBatchKey.GetHashCode() && key.Equals(this.lastBatchKey))
			{
				list = this.lastBatchList;
			}
			else
			{
				if (!this.batches.TryGetValue(key, out list))
				{
					list = ((this.batchListCache.Count == 0) ? new List<DrawBatch.BatchData>() : this.batchListCache.Pop<List<DrawBatch.BatchData>>());
					this.batches.Add(key, list);
					list.Add((this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>());
				}
				this.lastBatchList = list;
				this.lastBatchKey = key;
			}
			int index = list.Count - 1;
			if (list[index].ptr < 1023)
			{
				return list[index];
			}
			DrawBatch.BatchData batchData = (this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>();
			list.Add(batchData);
			return batchData;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00065A2C File Offset: 0x00063C2C
		public void MergeWith(DrawBatch other)
		{
			foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in other.batches)
			{
				foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
				{
					while (batchData.ptr > 0)
					{
						DrawBatch.BatchData batchDataForInsertion = this.GetBatchDataForInsertion(keyValuePair.Key);
						int num = Mathf.Min(batchData.ptr, 1023 - batchDataForInsertion.ptr);
						Array.Copy(batchData.matrices, 0, batchDataForInsertion.matrices, batchDataForInsertion.ptr, num);
						Array.Copy(batchData.colors, 0, batchDataForInsertion.colors, batchDataForInsertion.ptr, num);
						batchDataForInsertion.ptr += num;
						batchData.ptr -= num;
					}
				}
			}
		}

		// Token: 0x04000FF4 RID: 4084
		private Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>> batches = new Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>>();

		// Token: 0x04000FF5 RID: 4085
		private List<DrawBatch.BatchData> batchDataListCache = new List<DrawBatch.BatchData>();

		// Token: 0x04000FF6 RID: 4086
		private List<List<DrawBatch.BatchData>> batchListCache = new List<List<DrawBatch.BatchData>>();

		// Token: 0x04000FF7 RID: 4087
		private HashSet<DrawBatchPropertyBlock> myPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x04000FF8 RID: 4088
		private List<DrawBatchPropertyBlock> propertyBlockCache = new List<DrawBatchPropertyBlock>();

		// Token: 0x04000FF9 RID: 4089
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x04000FFA RID: 4090
		private HashSet<DrawBatchPropertyBlock> tmpPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x04000FFB RID: 4091
		public const int MaxCountPerBatch = 1023;

		// Token: 0x04000FFC RID: 4092
		private static bool PropertyBlockLeakDebug;

		// Token: 0x04000FFD RID: 4093
		private DrawBatch.BatchKey lastBatchKey;

		// Token: 0x04000FFE RID: 4094
		private List<DrawBatch.BatchData> lastBatchList;

		// Token: 0x020025CC RID: 9676
		private class BatchData
		{
			// Token: 0x0600F0E0 RID: 61664 RVA: 0x00589224 File Offset: 0x00587424
			public BatchData()
			{
				this.matrices = new Matrix4x4[1023];
				this.colors = new Vector4[1023];
				this.ptr = 0;
			}

			// Token: 0x0600F0E1 RID: 61665 RVA: 0x00589253 File Offset: 0x00587453
			public void Clear()
			{
				this.ptr = 0;
				this.hasAnyColors = false;
			}

			// Token: 0x0600F0E2 RID: 61666 RVA: 0x00589263 File Offset: 0x00587463
			public void Add(Matrix4x4 matrix)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = DrawBatch.BatchData.WhiteColor;
				this.ptr++;
			}

			// Token: 0x0600F0E3 RID: 61667 RVA: 0x0058929C File Offset: 0x0058749C
			public void Add(Matrix4x4 matrix, Color? color)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = (color ?? DrawBatch.BatchData.WhiteColor);
				this.ptr++;
				this.hasAnyColors = true;
			}

			// Token: 0x04009B03 RID: 39683
			public Matrix4x4[] matrices;

			// Token: 0x04009B04 RID: 39684
			public int ptr;

			// Token: 0x04009B05 RID: 39685
			public Vector4[] colors;

			// Token: 0x04009B06 RID: 39686
			public bool hasAnyColors;

			// Token: 0x04009B07 RID: 39687
			private static readonly Vector4 WhiteColor = Color.white;
		}

		// Token: 0x020025CD RID: 9677
		private struct BatchKey : IEquatable<DrawBatch.BatchKey>
		{
			// Token: 0x0600F0E5 RID: 61669 RVA: 0x00589318 File Offset: 0x00587518
			public BatchKey(Mesh mesh, Material material, int layer, bool renderInstanced, DrawBatchPropertyBlock propertyBlock)
			{
				this.mesh = mesh;
				this.material = material;
				this.layer = layer;
				this.renderInstanced = (renderInstanced && SystemInfo.supportsInstancing);
				this.propertyBlock = propertyBlock;
				this.hash = mesh.GetHashCode();
				this.hash = Gen.HashCombineInt(this.hash, material.GetHashCode());
				this.hash = Gen.HashCombineInt(this.hash, layer | ((((renderInstanced > false) ? 1 : 0) << 8) ? 1 : 0));
				this.hash = ((propertyBlock == null) ? this.hash : Gen.HashCombineInt(this.hash, propertyBlock.GetHashCode()));
			}

			// Token: 0x0600F0E6 RID: 61670 RVA: 0x005893B8 File Offset: 0x005875B8
			public override bool Equals(object obj)
			{
				if (obj != null && obj is DrawBatch.BatchKey)
				{
					DrawBatch.BatchKey other = (DrawBatch.BatchKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600F0E7 RID: 61671 RVA: 0x005893E4 File Offset: 0x005875E4
			public bool Equals(DrawBatch.BatchKey other)
			{
				return this.mesh == other.mesh && this.material == other.material && this.layer == other.layer && this.renderInstanced == other.renderInstanced && this.propertyBlock == other.propertyBlock;
			}

			// Token: 0x0600F0E8 RID: 61672 RVA: 0x00589439 File Offset: 0x00587639
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x04009B08 RID: 39688
			public readonly Mesh mesh;

			// Token: 0x04009B09 RID: 39689
			public readonly Material material;

			// Token: 0x04009B0A RID: 39690
			public readonly int layer;

			// Token: 0x04009B0B RID: 39691
			public readonly bool renderInstanced;

			// Token: 0x04009B0C RID: 39692
			public readonly DrawBatchPropertyBlock propertyBlock;

			// Token: 0x04009B0D RID: 39693
			private int hash;
		}
	}
}

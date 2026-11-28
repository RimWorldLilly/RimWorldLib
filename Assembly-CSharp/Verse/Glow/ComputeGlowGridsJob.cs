using System;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse.Glow
{
	// Token: 0x020007B6 RID: 1974
	[BurstCompile]
	public struct ComputeGlowGridsJob : IJobParallelFor
	{
		// Token: 0x06003F89 RID: 16265 RVA: 0x00196BD8 File Offset: 0x00194DD8
		[BurstCompile]
		public void Execute(int index)
		{
			GlowLight glowLight = this.lights[index];
			if (!glowLight.dirty)
			{
				return;
			}
			glowLight.dirty = false;
			this.lights[index] = glowLight;
			GlowUniqueState glowUniqueState = this.statePool[this.threadIndex];
			glowUniqueState.PrepareComparer(ref glowLight);
			LocalGlowArea localGlowArea = this.glowPool[glowLight.localGlowPoolIndex];
			this.PrepareFill(ref glowLight, ref glowUniqueState, ref localGlowArea);
			this.Flood(ref glowLight, ref glowUniqueState, ref localGlowArea);
			glowUniqueState.Clear();
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00196C5C File Offset: 0x00194E5C
		[BurstCompile]
		private void Flood(ref GlowLight glowLight, ref GlowUniqueState filler, ref LocalGlowArea localArea)
		{
			int num = Mathf.RoundToInt(glowLight.glowRadius * 100f);
			int num2 = glowLight.diameter * glowLight.diameter;
			IntVec3 a;
			while (filler.queue.TryPop(out a))
			{
				int index = glowLight.DeltaToLocalIndex(a);
				GlowCell value = filler.area[index];
				value.status = GlowCellStatus.Finalized;
				filler.area[index] = value;
				this.SetGlowFromDist(ref glowLight, ref filler, ref localArea, a);
				for (int i = 0; i < ComputeGlowGridsJob.Directions.Length; i++)
				{
					IntVec3 b = a + ComputeGlowGridsJob.Directions[i];
					int num3 = glowLight.DeltaToLocalIndex(b);
					if (num3 >= 0 && num3 < num2)
					{
						GlowCell glowCell = filler.area[num3];
						if (glowCell.status != GlowCellStatus.Finalized)
						{
							IntVec3 intVec = glowLight.position + b;
							if (intVec.x < this.mapSize.x && intVec.z < this.mapSize.z && intVec.x >= 0 && intVec.z >= 0)
							{
								bool flag = this.lightBlockers.IsSet(this.indices.CellToIndex(intVec));
								filler.blockers.Set(i, flag);
								if (!flag)
								{
									int num4 = (i < 4) ? 100 : 141;
									int num5 = filler.area[index].intDist + num4;
									if (num5 <= num)
									{
										if (i >= 4)
										{
											switch (i)
											{
											case 4:
												if (filler.blockers.IsSet(0) && filler.blockers.IsSet(1))
												{
													goto IL_278;
												}
												break;
											case 5:
												if (filler.blockers.IsSet(1) && filler.blockers.IsSet(2))
												{
													goto IL_278;
												}
												break;
											case 6:
												if (filler.blockers.IsSet(2) && filler.blockers.IsSet(3))
												{
													goto IL_278;
												}
												break;
											case 7:
												if (filler.blockers.IsSet(0) && filler.blockers.IsSet(3))
												{
													goto IL_278;
												}
												break;
											}
										}
										if (glowCell.status == GlowCellStatus.Unvisited)
										{
											glowCell.intDist = int.MaxValue;
											glowCell.status = GlowCellStatus.Open;
											filler.area[num3] = glowCell;
										}
										if (num5 < glowCell.intDist)
										{
											glowCell.intDist = num5;
											glowCell.status = GlowCellStatus.Open;
											filler.area[num3] = glowCell;
											filler.queue.Insert(b);
										}
									}
								}
							}
						}
					}
					IL_278:;
				}
			}
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x00196EFC File Offset: 0x001950FC
		[BurstCompile]
		private void PrepareFill(ref GlowLight glowLight, ref GlowUniqueState filler, ref LocalGlowArea localArea)
		{
			NativeArrayUtility.MemClear<Color32>(localArea.colors);
			IntVec3 intVec = new IntVec3(0, 0, 0);
			int num = glowLight.DeltaToLocalIndex(intVec);
			for (int i = 0; i < glowLight.diameter * glowLight.diameter; i++)
			{
				GlowCell value = default(GlowCell);
				if (i == num)
				{
					value.intDist = 100;
				}
				filler.area[i] = value;
			}
			filler.queue.Insert(IntVec3.Zero);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x00196F74 File Offset: 0x00195174
		[BurstCompile]
		private void SetGlowFromDist(ref GlowLight glowLight, ref GlowUniqueState filler, ref LocalGlowArea localArea, in IntVec3 delta)
		{
			float num = -1f / glowLight.glowRadius;
			int index = glowLight.DeltaToLocalIndex(delta);
			float num2 = (float)filler.area[index].intDist / 100f;
			ColorInt colorInt = default(ColorInt);
			if (num2 <= glowLight.glowRadius)
			{
				float b = 1f / (num2 * num2);
				float b2 = Mathf.Lerp(1f + num * num2, b, 0.4f);
				colorInt = glowLight.glowColor * b2;
				colorInt.a = 0;
			}
			if (colorInt.r <= 0 && colorInt.g <= 0 && colorInt.b <= 0)
			{
				return;
			}
			colorInt.ClampToNonNegative();
			colorInt.a = (int)num2;
			Color32 value;
			colorInt.ProjectToColor32Fast(out value);
			localArea.colors[index] = value;
		}

		// Token: 0x04002A0A RID: 10762
		private static readonly IntVec3[] Directions = new IntVec3[]
		{
			new IntVec3(0, 0, -1),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(-1, 0, 0),
			new IntVec3(1, 0, -1),
			new IntVec3(1, 0, 1),
			new IntVec3(-1, 0, 1),
			new IntVec3(-1, 0, -1)
		};

		// Token: 0x04002A0B RID: 10763
		private const int CardinalCost = 100;

		// Token: 0x04002A0C RID: 10764
		private const int DiagonalCost = 141;

		// Token: 0x04002A0D RID: 10765
		[NativeSetThreadIndex]
		[ReadOnly]
		private int threadIndex;

		// Token: 0x04002A0E RID: 10766
		[NativeDisableParallelForRestriction]
		public NativeList<GlowLight> lights;

		// Token: 0x04002A0F RID: 10767
		[ReadOnly]
		public NativeBitArray lightBlockers;

		// Token: 0x04002A10 RID: 10768
		[ReadOnly]
		public CellIndices indices;

		// Token: 0x04002A11 RID: 10769
		[NativeDisableParallelForRestriction]
		public NativeList<LocalGlowArea> glowPool;

		// Token: 0x04002A12 RID: 10770
		[NativeDisableParallelForRestriction]
		public NativeArray<GlowUniqueState> statePool;

		// Token: 0x04002A13 RID: 10771
		public IntVec3 mapSize;
	}
}

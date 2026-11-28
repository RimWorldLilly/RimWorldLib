using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000728 RID: 1832
	public struct ShootLine
	{
		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x060039DC RID: 14812 RVA: 0x00170AE6 File Offset: 0x0016ECE6
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x00170AEE File Offset: 0x0016ECEE
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x00170AF6 File Offset: 0x0016ECF6
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x00170B08 File Offset: 0x0016ED08
		public void ChangeDestToMissWild(float aimOnChance, bool flyOverhead, Map map)
		{
			float num = ShootTuning.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089);
			}
			IntVec3 a;
			do
			{
				Vector2 unitVector = Rand.UnitVector2;
				Vector3 b = new Vector3(unitVector.x * num, 0f, unitVector.y * num);
				a = (this.dest.ToVector3Shifted() + b).ToIntVec3();
			}
			while (Vector3.Dot((this.dest - this.source).ToVector3(), (a - this.source).ToVector3()) < 0f);
			this.dest = a;
			if (!flyOverhead && map != null && !ShootLeanUtility.CellCanSeeCell(this.source, this.dest, map))
			{
				foreach (IntVec3 intVec in this.Points())
				{
					this.dest = intVec;
					if (intVec != this.source && intVec.InBounds(map) && intVec.Filled(map) && !this.source.InHorDistOf(this.dest, 1.5f))
					{
						Building_Door door = intVec.GetDoor(map);
						if (door == null)
						{
							break;
						}
						if (!door.Open)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x00170C78 File Offset: 0x0016EE78
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x00170C8C File Offset: 0x0016EE8C
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			IntVec3 intVec = this.source;
			array[num] = intVec.ToString();
			array[2] = "->";
			int num2 = 3;
			intVec = this.dest;
			array[num2] = intVec.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x00170CEC File Offset: 0x0016EEEC
		[DebugOutput]
		public static void WildMissResults()
		{
			IntVec3 intVec = new IntVec3(100, 0, 0);
			ShootLine shootLine = new ShootLine(IntVec3.Zero, intVec);
			IEnumerable<int> enumerable = Enumerable.Range(0, 101);
			IEnumerable<int> colValues = Enumerable.Range(0, 12);
			int[,] results = new int[enumerable.Count<int>(), colValues.Count<int>()];
			foreach (int num in enumerable)
			{
				for (int i = 0; i < 10000; i++)
				{
					ShootLine shootLine2 = shootLine;
					shootLine2.ChangeDestToMissWild((float)num / 100f, false, null);
					if (shootLine2.dest.z == 0 && shootLine2.dest.x > intVec.x)
					{
						results[num, shootLine2.dest.x - intVec.x]++;
					}
				}
			}
			DebugTables.MakeTablesDialog<int, int>(colValues, (int cells) => cells.ToString() + "-away\ncell\nhit%", enumerable, (int hitchance) => ((float)hitchance / 100f).ToStringPercent() + " aimon chance", delegate(int cells, int hitchance)
			{
				float num2 = (float)hitchance / 100f;
				if (cells == 0)
				{
					return num2.ToStringPercent();
				}
				return ((float)results[hitchance, cells] / 10000f * (1f - num2)).ToStringPercent();
			}, "");
		}

		// Token: 0x0400261D RID: 9757
		private IntVec3 source;

		// Token: 0x0400261E RID: 9758
		private IntVec3 dest;
	}
}

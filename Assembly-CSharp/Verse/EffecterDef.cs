using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000167 RID: 359
	public class EffecterDef : Def
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x0003B917 File Offset: 0x00039B17
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0003B920 File Offset: 0x00039B20
		public Effecter SpawnMaintained(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = this.Spawn(target, map, scale);
			if (this.maintainTicks != 0)
			{
				map.effecterMaintainer.AddEffecterToMaintain(effecter, target, this.maintainTicks);
			}
			else
			{
				effecter.Cleanup();
			}
			return effecter;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0003B95C File Offset: 0x00039B5C
		public Effecter SpawnMaintained(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = this.Spawn(target, map, scale);
			if (this.maintainTicks != 0)
			{
				map.effecterMaintainer.AddEffecterToMaintain(effecter, target, this.maintainTicks);
			}
			else
			{
				effecter.Cleanup();
			}
			return effecter;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0003B998 File Offset: 0x00039B98
		public Effecter SpawnMaintained(TargetInfo A, TargetInfo B, float scale = 1f)
		{
			Effecter effecter = this.Spawn(A, B, scale);
			if (this.maintainTicks != 0)
			{
				A.Map.effecterMaintainer.AddEffecterToMaintain(effecter, A, B, this.maintainTicks);
			}
			else
			{
				effecter.Cleanup();
			}
			return effecter;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0003B9DC File Offset: 0x00039BDC
		public Effecter Spawn(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0003BA0C File Offset: 0x00039C0C
		public Effecter Spawn(IntVec3 targetA, IntVec3 targetB, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo a = new TargetInfo(targetA, map, false);
			effecter.scale = scale;
			effecter.Trigger(a, new TargetInfo(targetB, map, false), -1);
			return effecter;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0003BA44 File Offset: 0x00039C44
		public Effecter Spawn(IntVec3 target, Map map, Vector3 offset, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.offset = offset;
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0003BA7C File Offset: 0x00039C7C
		public Effecter Spawn(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = ((this.offsetMode == EffecterDef.OffsetMode.TrueCenterCompensated) ? (target.TrueCenter() - target.Position.ToVector3Shifted()) : Vector3.zero);
			effecter.scale = scale;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0003BADD File Offset: 0x00039CDD
		public Effecter Spawn(TargetInfo targetA, TargetInfo targetB, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.scale = scale;
			effecter.Trigger(targetA, targetB, -1);
			return effecter;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0003BAF8 File Offset: 0x00039CF8
		public Effecter SpawnAttached(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = ((this.offsetMode == EffecterDef.OffsetMode.TrueCenterCompensated) ? (target.TrueCenter() - target.Position.ToVector3Shifted()) : Vector3.zero);
			effecter.scale = scale;
			effecter.Trigger(target, target, -1);
			return effecter;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003BB54 File Offset: 0x00039D54
		public Effecter Spawn(Thing target, Map map, Vector3 offset)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = offset;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x04000801 RID: 2049
		public List<SubEffecterDef> children;

		// Token: 0x04000802 RID: 2050
		public float positionRadius;

		// Token: 0x04000803 RID: 2051
		public FloatRange offsetTowardsTarget;

		// Token: 0x04000804 RID: 2052
		public int maintainTicks;

		// Token: 0x04000805 RID: 2053
		public float randomWeight = 1f;

		// Token: 0x04000806 RID: 2054
		public EffecterDef.OffsetMode offsetMode;

		// Token: 0x0200253A RID: 9530
		public enum OffsetMode
		{
			// Token: 0x04009866 RID: 39014
			TrueCenterCompensated,
			// Token: 0x04009867 RID: 39015
			Free
		}
	}
}

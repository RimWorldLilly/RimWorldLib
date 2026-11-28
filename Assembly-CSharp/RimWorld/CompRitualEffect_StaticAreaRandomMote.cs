using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173D RID: 5949
	public class CompRitualEffect_StaticAreaRandomMote : CompRitualEffect_Constant
	{
		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x06009434 RID: 37940 RVA: 0x003605B3 File Offset: 0x0035E7B3
		protected CompProperties_RitualEffectStaticAreaRandomMote Props
		{
			get
			{
				return (CompProperties_RitualEffectStaticAreaRandomMote)this.props;
			}
		}

		// Token: 0x06009435 RID: 37941 RVA: 0x003605C0 File Offset: 0x0035E7C0
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x06009436 RID: 37942 RVA: 0x003605C7 File Offset: 0x0035E7C7
		protected override ThingDef MoteDef
		{
			get
			{
				return this.Props.moteDefs.RandomElement<ThingDef>();
			}
		}

		// Token: 0x06009437 RID: 37943 RVA: 0x003605DC File Offset: 0x0035E7DC
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			CellRect cellRect = CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map);
			List<IntVec3> list = new List<IntVec3>();
			for (int i = 0; i < this.Props.spawnCount; i++)
			{
				IntVec3 pos = IntVec3.Invalid;
				Predicate<IntVec3> <>9__0;
				for (int j = 0; j < 15; j++)
				{
					pos = cellRect.RandomCell;
					List<IntVec3> list2 = list;
					Predicate<IntVec3> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((IntVec3 c) => c.InHorDistOf(pos, this.Props.minDist)));
					}
					if (!list2.Any(predicate))
					{
						break;
					}
				}
				if (pos.IsValid)
				{
					Mote mote = this.SpawnMote(ritual, new Vector3?(pos.ToVector3Shifted() + this.Props.offset));
					if (mote != null)
					{
						parent.AddMoteToMaintain(mote);
					}
					list.Add(pos);
				}
			}
		}
	}
}

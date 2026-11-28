using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001468 RID: 5224
	public class CompAbilityEffect_Waterskip : CompAbilityEffect
	{
		// Token: 0x06008041 RID: 32833 RVA: 0x002DC110 File Offset: 0x002DA310
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			foreach (IntVec3 c in this.AffectedCells(target, map))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						if (thingList[i] is Fire)
						{
							thingList[i].Destroy(DestroyMode.Vanish);
						}
						else
						{
							Pawn pawn = thingList[i] as Pawn;
							if (pawn != null)
							{
								HediffComp_Invisibility invisibilityComp = pawn.GetInvisibilityComp();
								if (invisibilityComp != null)
								{
									invisibilityComp.DisruptInvisibility();
								}
							}
						}
					}
					if (!c.Filled(map))
					{
						FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Water, 1, FilthSourceFlags.None, true);
					}
					FleckCreationData dataStatic = FleckMaker.GetDataStatic(c.ToVector3Shifted(), map, FleckDefOf.WaterskipSplashParticles, 1f);
					dataStatic.rotationRate = (float)Rand.Range(-30, 30);
					dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
					map.flecks.CreateFleck(dataStatic);
					CompAbilityEffect_Teleport.SendSkipUsedSignal(c, this.parent.pawn);
				}
			}
		}

		// Token: 0x06008042 RID: 32834 RVA: 0x002DC26C File Offset: 0x002DA46C
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			CompAbilityEffect_Waterskip.<AffectedCells>d__1 <AffectedCells>d__ = new CompAbilityEffect_Waterskip.<AffectedCells>d__1(-2);
			<AffectedCells>d__.<>4__this = this;
			<AffectedCells>d__.<>3__target = target;
			<AffectedCells>d__.<>3__map = map;
			return <AffectedCells>d__;
		}

		// Token: 0x06008043 RID: 32835 RVA: 0x002DC28C File Offset: 0x002DA48C
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null, null, 2900);
		}

		// Token: 0x06008044 RID: 32836 RVA: 0x002DC2E0 File Offset: 0x002DA4E0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000738 RID: 1848
	public static class VerbUtility
	{
		// Token: 0x06003A95 RID: 14997 RVA: 0x00175B34 File Offset: 0x00173D34
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			if (verb_LaunchProjectile == null)
			{
				return null;
			}
			return verb_LaunchProjectile.Projectile;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00175B54 File Offset: 0x00173D54
		public static DamageDef GetDamageDef(this Verb verb)
		{
			if (!verb.verbProps.LaunchesProjectile)
			{
				return verb.verbProps.meleeDamageDef;
			}
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null)
			{
				return projectile.projectile.damageDef;
			}
			return null;
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x00175B94 File Offset: 0x00173D94
		public static bool IsIncendiary_Ranged(this Verb verb)
		{
			if (verb.verbProps != null && verb.verbProps.ai_BeamIsIncendiary)
			{
				return true;
			}
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x00175BD0 File Offset: 0x00173DD0
		public static bool IsIncendiary_Melee(this Verb verb)
		{
			if (verb.tool != null && !verb.tool.extraMeleeDamages.NullOrEmpty<ExtraDamage>())
			{
				for (int i = 0; i < verb.tool.extraMeleeDamages.Count; i++)
				{
					if (verb.tool.extraMeleeDamages[i].def == DamageDefOf.Flame)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x00175C34 File Offset: 0x00173E34
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x00175C58 File Offset: 0x00173E58
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x00175C77 File Offset: 0x00173E77
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x00175C88 File Offset: 0x00173E88
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x00175CB4 File Offset: 0x00173EB4
		public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = ((Pawn)concreteExample).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				result = hediffDef.ConcreteExample.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x00175D30 File Offset: 0x00173F30
		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
			if (num < 9f)
			{
				return 0f;
			}
			if (num < 25f)
			{
				return forcedMiss * 0.5f;
			}
			if (num < 49f)
			{
				return forcedMiss * 0.8f;
			}
			return forcedMiss;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x00175D78 File Offset: 0x00173F78
		public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			if (num <= 25f)
			{
				return 0f;
			}
			if (num >= 144f)
			{
				return 1f;
			}
			return Mathf.InverseLerp(25f, 144f, num);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00175DC4 File Offset: 0x00173FC4
		public static IEnumerable<VerbUtility.VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
		{
			VerbUtility.<GetAllVerbProperties>d__12 <GetAllVerbProperties>d__ = new VerbUtility.<GetAllVerbProperties>d__12(-2);
			<GetAllVerbProperties>d__.<>3__verbProps = verbProps;
			<GetAllVerbProperties>d__.<>3__tools = tools;
			return <GetAllVerbProperties>d__;
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x00175DDC File Offset: 0x00173FDC
		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			if (!(caster is Pawn))
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00175E14 File Offset: 0x00174014
		public static VerbSelectionCategory GetSelectionCategory(this Verb v, Pawn p, float highestWeight)
		{
			return VerbUtility.SelectionCategoryForWeight(VerbUtility.InitialVerbWeight(v, p), highestWeight);
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00175E23 File Offset: 0x00174023
		public static VerbSelectionCategory SelectionCategoryForWeight(float selWeight, float highestWeight)
		{
			if (selWeight >= highestWeight * 0.95f)
			{
				return VerbSelectionCategory.Best;
			}
			if (selWeight < highestWeight * 0.25f)
			{
				return VerbSelectionCategory.Worst;
			}
			return VerbSelectionCategory.Mid;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00175E3E File Offset: 0x0017403E
		public static float InitialVerbWeight(Verb v, Pawn p)
		{
			return VerbUtility.DPS(v, p) * VerbUtility.AdditionalSelectionFactor(v);
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x00175E4E File Offset: 0x0017404E
		public static float DPS(Verb v, Pawn p)
		{
			return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x00175E8C File Offset: 0x0017408C
		private static float AdditionalSelectionFactor(Verb v)
		{
			float num = (v.tool != null) ? v.tool.chanceFactor : 1f;
			if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
			{
				foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
				{
					num += 0.1f;
				}
			}
			return num;
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x00175F2C File Offset: 0x0017412C
		public static float FinalSelectionWeight(Verb verb, Pawn p, List<Verb> allMeleeVerbs, float highestWeight)
		{
			VerbSelectionCategory selectionCategory = verb.GetSelectionCategory(p, highestWeight);
			if (selectionCategory == VerbSelectionCategory.Worst)
			{
				return 0f;
			}
			int num = 0;
			using (List<Verb>.Enumerator enumerator = allMeleeVerbs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetSelectionCategory(p, highestWeight) == selectionCategory)
					{
						num++;
					}
				}
			}
			return VerbUtility.FinalSelectionWeightFor(selectionCategory, num);
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00175F9C File Offset: 0x0017419C
		public static float FinalSelectionWeightFor(VerbSelectionCategory category, int numInSameCategory)
		{
			if (category == VerbSelectionCategory.Worst)
			{
				return 0f;
			}
			return 1f / (float)numInSameCategory * ((category == VerbSelectionCategory.Mid) ? 0.25f : 0.75f);
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x00175FC4 File Offset: 0x001741C4
		public static List<Thing> ThingsToHit(IntVec3 cell, Map map, Func<Thing, bool> filter)
		{
			VerbUtility.cellThingsFiltered.Clear();
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && filter(thing))
				{
					VerbUtility.cellThingsFiltered.Add(thing);
				}
			}
			return VerbUtility.cellThingsFiltered;
		}

		// Token: 0x0400269D RID: 9885
		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();

		// Token: 0x02002A80 RID: 10880
		public struct VerbPropertiesWithSource
		{
			// Token: 0x17002BDB RID: 11227
			// (get) Token: 0x060106AA RID: 67242 RVA: 0x005C7257 File Offset: 0x005C5457
			public ToolCapacityDef ToolCapacity
			{
				get
				{
					if (this.maneuver == null)
					{
						return null;
					}
					return this.maneuver.requiredCapacity;
				}
			}

			// Token: 0x060106AB RID: 67243 RVA: 0x005C726E File Offset: 0x005C546E
			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			// Token: 0x060106AC RID: 67244 RVA: 0x005C7285 File Offset: 0x005C5485
			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			// Token: 0x0400AE18 RID: 44568
			public VerbProperties verbProps;

			// Token: 0x0400AE19 RID: 44569
			public Tool tool;

			// Token: 0x0400AE1A RID: 44570
			public ManeuverDef maneuver;
		}
	}
}

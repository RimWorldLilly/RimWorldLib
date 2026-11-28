using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200011C RID: 284
	public class BodyPartDef : Def
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00036025 File Offset: 0x00034225
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0003602D File Offset: 0x0003422D
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00036035 File Offset: 0x00034235
		public bool IsMirroredPart
		{
			get
			{
				return this.tags.Contains(BodyPartTagDefOf.Mirrored);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00036047 File Offset: 0x00034247
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00036063 File Offset: 0x00034263
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0003609D File Offset: 0x0003429D
		public override IEnumerable<string> ConfigErrors()
		{
			BodyPartDef.<ConfigErrors>d__34 <ConfigErrors>d__ = new BodyPartDef.<ConfigErrors>d__34(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000360B0 File Offset: 0x000342B0
		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid && !hediffs[i].def.organicAddedBodypart;
					}
				}
			}
			return this.solid;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0003612E File Offset: 0x0003432E
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00036141 File Offset: 0x00034341
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00036158 File Offset: 0x00034358
		public float GetHitChanceFactorFor(DamageDef damage)
		{
			if (this.conceptual)
			{
				return 0f;
			}
			if (this.hitChanceFactors == null)
			{
				return 1f;
			}
			float result;
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}

		// Token: 0x040004A2 RID: 1186
		[MustTranslate]
		public string labelShort;

		// Token: 0x040004A3 RID: 1187
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x040004A4 RID: 1188
		public int hitPoints = 10;

		// Token: 0x040004A5 RID: 1189
		public float permanentInjuryChanceFactor = 1f;

		// Token: 0x040004A6 RID: 1190
		public float bleedRate = 1f;

		// Token: 0x040004A7 RID: 1191
		public float frostbiteVulnerability;

		// Token: 0x040004A8 RID: 1192
		private bool skinCovered;

		// Token: 0x040004A9 RID: 1193
		private bool solid;

		// Token: 0x040004AA RID: 1194
		public bool alive = true;

		// Token: 0x040004AB RID: 1195
		public bool delicate;

		// Token: 0x040004AC RID: 1196
		public bool canScarify;

		// Token: 0x040004AD RID: 1197
		public bool beautyRelated;

		// Token: 0x040004AE RID: 1198
		public bool conceptual;

		// Token: 0x040004AF RID: 1199
		public bool socketed;

		// Token: 0x040004B0 RID: 1200
		public ThingDef spawnThingOnRemoved;

		// Token: 0x040004B1 RID: 1201
		public bool pawnGeneratorCanAmputate;

		// Token: 0x040004B2 RID: 1202
		public bool canSuggestAmputation = true;

		// Token: 0x040004B3 RID: 1203
		public bool forceAlwaysRemovable;

		// Token: 0x040004B4 RID: 1204
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x040004B5 RID: 1205
		public bool destroyableByDamage = true;

		// Token: 0x040004B6 RID: 1206
		public bool canBeVacuumBurnt = true;

		// Token: 0x040004B7 RID: 1207
		[MustTranslate]
		public string removeRecipeLabelOverride;

		// Token: 0x040004B8 RID: 1208
		public float executionPartPriority;

		// Token: 0x040004B9 RID: 1209
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}

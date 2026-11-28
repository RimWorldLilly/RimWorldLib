using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001903 RID: 6403
	public class FocusStrengthOffset_BuildingDefs : FocusStrengthOffset
	{
		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x0600A07F RID: 41087 RVA: 0x00396B68 File Offset: 0x00394D68
		public float MaxOffsetPerBuilding
		{
			get
			{
				if (this.maxOffsetPerBuilding == -3.4028235E+38f)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs[i];
						if (meditationFocusOffsetPerBuilding.offset > this.maxOffsetPerBuilding)
						{
							this.maxOffsetPerBuilding = meditationFocusOffsetPerBuilding.offset;
						}
					}
				}
				return this.maxOffsetPerBuilding;
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x0600A080 RID: 41088 RVA: 0x00396BC8 File Offset: 0x00394DC8
		public float MinOffsetPerBuilding
		{
			get
			{
				if (this.minOffsetPerBuilding == 3.4028235E+38f)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs[i];
						if (meditationFocusOffsetPerBuilding.offset < this.minOffsetPerBuilding)
						{
							this.minOffsetPerBuilding = meditationFocusOffsetPerBuilding.offset;
						}
					}
				}
				return this.minOffsetPerBuilding;
			}
		}

		// Token: 0x0600A081 RID: 41089 RVA: 0x00396C28 File Offset: 0x00394E28
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				float num = 0f;
				List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
				int num2 = 0;
				while (num2 < forCell.Count && num2 < this.maxBuildings)
				{
					num += this.OffsetForBuilding(forCell[num2]);
					num2++;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x0600A082 RID: 41090 RVA: 0x00396C98 File Offset: 0x00394E98
		protected virtual int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			return Math.Min(parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent).Count, this.maxBuildings);
		}

		// Token: 0x0600A083 RID: 41091 RVA: 0x00396CE5 File Offset: 0x00394EE5
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned && this.BuildingCount(parent) > 0;
		}

		// Token: 0x0600A084 RID: 41092 RVA: 0x00396CFB File Offset: 0x00394EFB
		protected virtual float OffsetForBuilding(Thing b)
		{
			return this.OffsetFor(b.def);
		}

		// Token: 0x0600A085 RID: 41093 RVA: 0x00396D0C File Offset: 0x00394F0C
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			if (this.drawRingRadius)
			{
				GenDraw.DrawRadiusRing(parent.Position, this.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
			}
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			int num = 0;
			while (num < forCell.Count && num < this.maxBuildings)
			{
				GenDraw.DrawLineBetween(parent.TrueCenter(), forCell[num].TrueCenter(), SimpleColor.Green, 0.2f);
				num++;
			}
		}

		// Token: 0x0600A086 RID: 41094 RVA: 0x00396D9C File Offset: 0x00394F9C
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%"), this.defs[0].building.Named("BUILDING")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x0600A087 RID: 41095 RVA: 0x00396E54 File Offset: 0x00395054
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%"), this.defs[0].building.Named("BUILDING")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x0600A088 RID: 41096 RVA: 0x00396EEA File Offset: 0x003950EA
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.MaxOffsetPerBuilding;
		}

		// Token: 0x0600A089 RID: 41097 RVA: 0x00396EFC File Offset: 0x003950FC
		protected float OffsetFor(ThingDef def)
		{
			MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == def);
			if (meditationFocusOffsetPerBuilding != null)
			{
				return meditationFocusOffsetPerBuilding.offset;
			}
			return 0f;
		}

		// Token: 0x0600A08A RID: 41098 RVA: 0x00396F40 File Offset: 0x00395140
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding in this.defs)
			{
				if (meditationFocusOffsetPerBuilding.offset == -3.4028235E+38f)
				{
					meditationFocusOffsetPerBuilding.offset = this.offsetPerBuilding;
				}
			}
		}

		// Token: 0x04005ECB RID: 24267
		public List<MeditationFocusOffsetPerBuilding> defs = new List<MeditationFocusOffsetPerBuilding>();

		// Token: 0x04005ECC RID: 24268
		public float radius = 10f;

		// Token: 0x04005ECD RID: 24269
		public int maxBuildings = int.MaxValue;

		// Token: 0x04005ECE RID: 24270
		public float offsetPerBuilding;

		// Token: 0x04005ECF RID: 24271
		public bool drawRingRadius = true;

		// Token: 0x04005ED0 RID: 24272
		[NoTranslate]
		public string explanationKey;

		// Token: 0x04005ED1 RID: 24273
		[NoTranslate]
		public string explanationKeyAbstract;

		// Token: 0x04005ED2 RID: 24274
		protected float minOffsetPerBuilding = float.MaxValue;

		// Token: 0x04005ED3 RID: 24275
		protected float maxOffsetPerBuilding = float.MinValue;
	}
}

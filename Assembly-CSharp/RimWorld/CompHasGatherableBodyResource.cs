using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B6 RID: 6326
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x06009E96 RID: 40598
		protected abstract int GatherResourcesIntervalDays { get; }

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x06009E97 RID: 40599
		protected abstract int ResourceAmount { get; }

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x06009E98 RID: 40600
		protected abstract ThingDef ResourceDef { get; }

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x06009E99 RID: 40601
		protected abstract string SaveKey { get; }

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x06009E9A RID: 40602 RVA: 0x0038EBDE File Offset: 0x0038CDDE
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x06009E9B RID: 40603 RVA: 0x0038EBE6 File Offset: 0x0038CDE6
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null && !this.parent.Suspended;
			}
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x06009E9C RID: 40604 RVA: 0x0038EC07 File Offset: 0x0038CE07
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		// Token: 0x06009E9D RID: 40605 RVA: 0x0038EC23 File Offset: 0x0038CE23
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		// Token: 0x06009E9E RID: 40606 RVA: 0x0038EC44 File Offset: 0x0038CE44
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (float)(this.GatherResourcesIntervalDays * 60000);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.fullness += num;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		// Token: 0x06009E9F RID: 40607 RVA: 0x0038ECAC File Offset: 0x0038CEAC
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				string str = (doer != null) ? doer.ToString() : null;
				string str2 = " gathered body resources while not Active: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + str2 + ((parent != null) ? parent.ToString() : null));
			}
			if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield, true, -1)))
			{
				MoteMaker.ThrowText((doer.DrawPos + this.parent.DrawPos) / 2f, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int i = GenMath.RoundRandom((float)this.ResourceAmount * this.fullness);
				while (i > 0)
				{
					int num = Mathf.Clamp(i, 1, this.ResourceDef.stackLimit);
					i -= num;
					Thing thing = ThingMaker.MakeThing(this.ResourceDef, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			this.fullness = 0f;
		}

		// Token: 0x04005DE0 RID: 24032
		protected float fullness;
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159E RID: 5534
	public class Pawn_NativeVerbs : IVerbOwner, IExposable
	{
		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x06008861 RID: 34913 RVA: 0x00315AA0 File Offset: 0x00313CA0
		public Verb_BeatFire BeatFireVerb
		{
			get
			{
				if (this.cachedBeatFireVerb == null)
				{
					this.cachedBeatFireVerb = (Verb_BeatFire)this.verbTracker.GetVerb(VerbCategory.BeatFire);
				}
				return this.cachedBeatFireVerb;
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x06008862 RID: 34914 RVA: 0x00315AC7 File Offset: 0x00313CC7
		public Verb_Ignite IgniteVerb
		{
			get
			{
				if (this.cachedIgniteVerb == null)
				{
					this.cachedIgniteVerb = (Verb_Ignite)this.verbTracker.GetVerb(VerbCategory.Ignite);
				}
				return this.cachedIgniteVerb;
			}
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06008863 RID: 34915 RVA: 0x00315AEE File Offset: 0x00313CEE
		VerbTracker IVerbOwner.VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06008864 RID: 34916 RVA: 0x00315AF6 File Offset: 0x00313CF6
		List<VerbProperties> IVerbOwner.VerbProperties
		{
			get
			{
				this.CheckCreateVerbProperties();
				return this.cachedVerbProperties;
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06008865 RID: 34917 RVA: 0x00315B04 File Offset: 0x00313D04
		List<Tool> IVerbOwner.Tools
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06008866 RID: 34918 RVA: 0x00315B07 File Offset: 0x00313D07
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x06008867 RID: 34919 RVA: 0x00315B0E File Offset: 0x00313D0E
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "NativeVerbs_" + this.pawn.ThingID;
		}

		// Token: 0x06008868 RID: 34920 RVA: 0x00315B25 File Offset: 0x00313D25
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.pawn;
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06008869 RID: 34921 RVA: 0x00315B30 File Offset: 0x00313D30
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600886A RID: 34922 RVA: 0x00315B38 File Offset: 0x00313D38
		private Thing ConstantCaster { get; }

		// Token: 0x0600886B RID: 34923 RVA: 0x00315B40 File Offset: 0x00313D40
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x0600886C RID: 34924 RVA: 0x00315B5B File Offset: 0x00313D5B
		public void NativeVerbsTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x0600886D RID: 34925 RVA: 0x00315B68 File Offset: 0x00313D68
		public bool TryStartIgnite(Thing target)
		{
			if (this.IgniteVerb == null)
			{
				Pawn pawn = this.pawn;
				Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " tried to ignite " + ((target != null) ? target.ToString() : null) + " but has no ignite verb.", 76453432);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.IgniteVerb.TryStartCastOn(target, false, true, false, false);
		}

		// Token: 0x0600886E RID: 34926 RVA: 0x00315BE0 File Offset: 0x00313DE0
		public bool TryBeatFire(Fire targetFire)
		{
			if (this.BeatFireVerb == null)
			{
				Pawn pawn = this.pawn;
				Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " tried to beat fire " + ((targetFire != null) ? targetFire.ToString() : null) + " but has no beat fire verb.", 935137531);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.BeatFireVerb.TryStartCastOn(targetFire, false, true, false, false);
		}

		// Token: 0x0600886F RID: 34927 RVA: 0x00315C58 File Offset: 0x00313E58
		public void ExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008870 RID: 34928 RVA: 0x00315C7C File Offset: 0x00313E7C
		private void CheckCreateVerbProperties()
		{
			if (this.cachedVerbProperties != null)
			{
				return;
			}
			if (this.pawn.RaceProps.intelligence >= Intelligence.ToolUser || this.pawn.RaceProps.giveNonToolUserBeatFireVerb)
			{
				this.cachedVerbProperties = new List<VerbProperties>();
				this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
				if (!this.pawn.RaceProps.IsMechanoid && !this.pawn.RaceProps.disableIgniteVerb && this.pawn.RaceProps.intelligence >= Intelligence.ToolUser)
				{
					this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite));
				}
			}
		}

		// Token: 0x04005418 RID: 21528
		private Pawn pawn;

		// Token: 0x04005419 RID: 21529
		public VerbTracker verbTracker;

		// Token: 0x0400541A RID: 21530
		private Verb_BeatFire cachedBeatFireVerb;

		// Token: 0x0400541B RID: 21531
		private Verb_Ignite cachedIgniteVerb;

		// Token: 0x0400541C RID: 21532
		private List<VerbProperties> cachedVerbProperties;
	}
}

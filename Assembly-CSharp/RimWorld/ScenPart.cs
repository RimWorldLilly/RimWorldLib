using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001797 RID: 6039
	public abstract class ScenPart : IExposable
	{
		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x06009676 RID: 38518 RVA: 0x00369FA1 File Offset: 0x003681A1
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x06009677 RID: 38519 RVA: 0x00369FA8 File Offset: 0x003681A8
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x06009678 RID: 38520 RVA: 0x00369FBA File Offset: 0x003681BA
		public virtual bool OverrideDangerMusic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009679 RID: 38521 RVA: 0x00369FBD File Offset: 0x003681BD
		public virtual bool Valid()
		{
			return this.def != null;
		}

		// Token: 0x0600967A RID: 38522 RVA: 0x00369FC8 File Offset: 0x003681C8
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x0600967B RID: 38523 RVA: 0x00369FDA File Offset: 0x003681DA
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x0600967C RID: 38524 RVA: 0x00369FEE File Offset: 0x003681EE
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x0600967D RID: 38525 RVA: 0x00369FFB File Offset: 0x003681FB
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x0600967E RID: 38526 RVA: 0x0036A00A File Offset: 0x0036820A
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x0600967F RID: 38527 RVA: 0x0036A017 File Offset: 0x00368217
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06009680 RID: 38528 RVA: 0x0036A01E File Offset: 0x0036821E
		public virtual void Randomize()
		{
		}

		// Token: 0x06009681 RID: 38529 RVA: 0x0036A020 File Offset: 0x00368220
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x06009682 RID: 38530 RVA: 0x0036A023 File Offset: 0x00368223
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x06009683 RID: 38531 RVA: 0x0036A026 File Offset: 0x00368226
		public virtual IEnumerable<Page> GetConfigPages()
		{
			return Enumerable.Empty<Page>();
		}

		// Token: 0x06009684 RID: 38532 RVA: 0x0036A02D File Offset: 0x0036822D
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x06009685 RID: 38533 RVA: 0x0036A030 File Offset: 0x00368230
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x06009686 RID: 38534 RVA: 0x0036A032 File Offset: 0x00368232
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x06009687 RID: 38535 RVA: 0x0036A034 File Offset: 0x00368234
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x06009688 RID: 38536 RVA: 0x0036A036 File Offset: 0x00368236
		public virtual void PreConfigure()
		{
		}

		// Token: 0x06009689 RID: 38537 RVA: 0x0036A038 File Offset: 0x00368238
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x0600968A RID: 38538 RVA: 0x0036A03A File Offset: 0x0036823A
		public virtual void PostIdeoChosen()
		{
		}

		// Token: 0x0600968B RID: 38539 RVA: 0x0036A03C File Offset: 0x0036823C
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x0600968C RID: 38540 RVA: 0x0036A03E File Offset: 0x0036823E
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600968D RID: 38541 RVA: 0x0036A045 File Offset: 0x00368245
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x0600968E RID: 38542 RVA: 0x0036A047 File Offset: 0x00368247
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x0600968F RID: 38543 RVA: 0x0036A049 File Offset: 0x00368249
		public virtual void PostGravshipLanded(Map map)
		{
		}

		// Token: 0x06009690 RID: 38544 RVA: 0x0036A04B File Offset: 0x0036824B
		public virtual void MapRemoved(Map map)
		{
		}

		// Token: 0x06009691 RID: 38545 RVA: 0x0036A04D File Offset: 0x0036824D
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06009692 RID: 38546 RVA: 0x0036A04F File Offset: 0x0036824F
		public virtual void Tick()
		{
		}

		// Token: 0x06009693 RID: 38547 RVA: 0x0036A051 File Offset: 0x00368251
		public virtual IEnumerable<Alert> GetAlerts()
		{
			return Enumerable.Empty<Alert>();
		}

		// Token: 0x06009694 RID: 38548 RVA: 0x0036A058 File Offset: 0x00368258
		public virtual IEnumerable<string> ConfigErrors()
		{
			ScenPart.<ConfigErrors>d__36 <ConfigErrors>d__ = new ScenPart.<ConfigErrors>d__36(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06009695 RID: 38549 RVA: 0x0036A068 File Offset: 0x00368268
		public virtual bool HasNullDefs()
		{
			return this.def == null;
		}

		// Token: 0x06009696 RID: 38550 RVA: 0x0036A073 File Offset: 0x00368273
		public override int GetHashCode()
		{
			if (this.def == null)
			{
				return 0;
			}
			return this.def.GetHashCode();
		}

		// Token: 0x04005A4D RID: 23117
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x04005A4E RID: 23118
		public bool visible = true;

		// Token: 0x04005A4F RID: 23119
		public bool summarized;
	}
}

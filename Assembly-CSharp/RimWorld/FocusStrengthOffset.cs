using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F9 RID: 6393
	public abstract class FocusStrengthOffset
	{
		// Token: 0x0600A045 RID: 41029 RVA: 0x003964AA File Offset: 0x003946AA
		public virtual string GetExplanation(Thing parent)
		{
			return "";
		}

		// Token: 0x0600A046 RID: 41030 RVA: 0x003964B1 File Offset: 0x003946B1
		public virtual string GetExplanationAbstract(ThingDef def = null)
		{
			return "";
		}

		// Token: 0x0600A047 RID: 41031 RVA: 0x003964B8 File Offset: 0x003946B8
		public virtual string InspectStringExtra(Thing parent, Pawn user = null)
		{
			return "";
		}

		// Token: 0x0600A048 RID: 41032 RVA: 0x003964BF File Offset: 0x003946BF
		public virtual float GetOffset(Thing parent, Pawn user = null)
		{
			return 0f;
		}

		// Token: 0x0600A049 RID: 41033 RVA: 0x003964C6 File Offset: 0x003946C6
		public virtual bool CanApply(Thing parent, Pawn user = null)
		{
			return true;
		}

		// Token: 0x0600A04A RID: 41034 RVA: 0x003964C9 File Offset: 0x003946C9
		public virtual void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
		}

		// Token: 0x0600A04B RID: 41035 RVA: 0x003964CB File Offset: 0x003946CB
		public virtual float MinOffset(Thing parent = null)
		{
			return 0f;
		}

		// Token: 0x0600A04C RID: 41036 RVA: 0x003964D2 File Offset: 0x003946D2
		public virtual float MaxOffset(Thing parent = null)
		{
			return this.offset;
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x0600A04D RID: 41037 RVA: 0x003964DA File Offset: 0x003946DA
		public virtual bool DependsOnPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x0600A04E RID: 41038 RVA: 0x003964DD File Offset: 0x003946DD
		public virtual bool NeedsToBeSpawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A04F RID: 41039 RVA: 0x003964E0 File Offset: 0x003946E0
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x04005EC6 RID: 24262
		public float offset;
	}
}

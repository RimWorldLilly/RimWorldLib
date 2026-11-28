using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E6 RID: 6630
	public class CompEntityHolderPlatform : CompEntityHolder
	{
		// Token: 0x17001D39 RID: 7481
		// (get) Token: 0x0600A6EF RID: 42735 RVA: 0x003B7C4B File Offset: 0x003B5E4B
		public override bool Available
		{
			get
			{
				return !base.HoldingPlatform.Occupied;
			}
		}

		// Token: 0x17001D3A RID: 7482
		// (get) Token: 0x0600A6F0 RID: 42736 RVA: 0x003B7C5B File Offset: 0x003B5E5B
		public override Pawn HeldPawn
		{
			get
			{
				return base.HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x17001D3B RID: 7483
		// (get) Token: 0x0600A6F1 RID: 42737 RVA: 0x003B7C68 File Offset: 0x003B5E68
		public override ThingOwner Container
		{
			get
			{
				return base.HoldingPlatform.innerContainer;
			}
		}

		// Token: 0x17001D3C RID: 7484
		// (get) Token: 0x0600A6F2 RID: 42738 RVA: 0x003B7C75 File Offset: 0x003B5E75
		public new CompProperties_EntityHolderPlatform Props
		{
			get
			{
				return (CompProperties_EntityHolderPlatform)this.props;
			}
		}

		// Token: 0x0600A6F3 RID: 42739 RVA: 0x003B7C82 File Offset: 0x003B5E82
		public override void EjectContents()
		{
			base.HoldingPlatform.EjectContents();
		}
	}
}

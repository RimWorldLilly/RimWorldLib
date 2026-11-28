using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F1 RID: 6641
	public class CompGrayStatueTeleporter : CompGrayStatue
	{
		// Token: 0x17001D49 RID: 7497
		// (get) Token: 0x0600A737 RID: 42807 RVA: 0x003B91B9 File Offset: 0x003B73B9
		private CompProperties_GrayStatueTeleporter Props
		{
			get
			{
				return (CompProperties_GrayStatueTeleporter)this.props;
			}
		}

		// Token: 0x0600A738 RID: 42808 RVA: 0x003B91C6 File Offset: 0x003B73C6
		protected override void Trigger(Pawn target)
		{
			this.parent.MapHeld.GetComponent<LabyrinthMapComponent>().TeleportToLabyrinth(target);
		}
	}
}

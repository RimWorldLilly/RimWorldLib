using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001922 RID: 6434
	public class CompObelisk_Labyrinth : CompInteractable
	{
		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x0600A166 RID: 41318 RVA: 0x0039B06E File Offset: 0x0039926E
		public new CompProperties_ObeliskLabyrinth Props
		{
			get
			{
				return (CompProperties_ObeliskLabyrinth)this.props;
			}
		}

		// Token: 0x0600A167 RID: 41319 RVA: 0x0039B07B File Offset: 0x0039927B
		protected override void OnInteracted(Pawn caster)
		{
			Messages.Message(this.Props.messageActivating, this.parent, MessageTypeDefOf.NeutralEvent, false);
			this.parent.Map.GetComponent<LabyrinthMapComponent>().StartClosing();
		}
	}
}

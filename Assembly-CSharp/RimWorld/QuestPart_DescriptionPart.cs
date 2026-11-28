using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B4 RID: 4276
	public class QuestPart_DescriptionPart : QuestPartActivable
	{
		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06006AFD RID: 27389 RVA: 0x002591D8 File Offset: 0x002573D8
		public override string DescriptionPart
		{
			get
			{
				return this.resolvedDescriptionPart;
			}
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x002591E0 File Offset: 0x002573E0
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedDescriptionPart = receivedArgs.GetFormattedText(this.descriptionPart).Resolve();
		}

		// Token: 0x06006AFF RID: 27391 RVA: 0x00259214 File Offset: 0x00257414
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.descriptionPart, "descriptionPart", null, false);
			Scribe_Values.Look<string>(ref this.resolvedDescriptionPart, "resolvedDescriptionPart", null, false);
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x00259240 File Offset: 0x00257440
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.descriptionPart = "Debug description part.";
		}

		// Token: 0x04004404 RID: 17412
		public string descriptionPart;

		// Token: 0x04004405 RID: 17413
		protected string resolvedDescriptionPart;
	}
}

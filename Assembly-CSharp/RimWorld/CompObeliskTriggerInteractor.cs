using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191D RID: 6429
	public class CompObeliskTriggerInteractor : CompInteractable
	{
		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x0600A13A RID: 41274 RVA: 0x00399E58 File Offset: 0x00398058
		private CompObelisk ObeliskComp
		{
			get
			{
				CompObelisk result;
				if ((result = this.obeliskComp) == null)
				{
					result = (this.obeliskComp = this.parent.GetComp<CompObelisk>());
				}
				return result;
			}
		}

		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x0600A13B RID: 41275 RVA: 0x00399E83 File Offset: 0x00398083
		public override string ExposeKey
		{
			get
			{
				return "Interactor";
			}
		}

		// Token: 0x0600A13C RID: 41276 RVA: 0x00399E8A File Offset: 0x0039808A
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (this.ObeliskComp.StudyLevel < 2 || this.ObeliskComp.ActivityComp.Deactivated || this.ObeliskComp.Activated)
			{
				return false;
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x0600A13D RID: 41277 RVA: 0x00399EC8 File Offset: 0x003980C8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompObeliskTriggerInteractor.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompObeliskTriggerInteractor.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A13E RID: 41278 RVA: 0x00399ED8 File Offset: 0x003980D8
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompObeliskTriggerInteractor.<CompFloatMenuOptions>d__8 <CompFloatMenuOptions>d__ = new CompObeliskTriggerInteractor.<CompFloatMenuOptions>d__8(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A13F RID: 41279 RVA: 0x00399EEF File Offset: 0x003980EF
		protected override void OnInteracted(Pawn caster)
		{
			if (this.obeliskComp.Activated)
			{
				return;
			}
			this.ObeliskComp.TriggerInteractionEffect(caster, true);
		}

		// Token: 0x04005F33 RID: 24371
		private CompObelisk obeliskComp;

		// Token: 0x04005F34 RID: 24372
		private const int StudyNeeded = 2;
	}
}

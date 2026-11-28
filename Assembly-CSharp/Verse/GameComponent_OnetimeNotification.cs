using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001ED RID: 493
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x06000DF4 RID: 3572 RVA: 0x00048368 File Offset: 0x00046568
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00048378 File Offset: 0x00046578
		public override void GameComponentTick()
		{
			if (Find.TickManager.TicksGame % 2000 != 0 || !Rand.Chance(0.05f))
			{
				return;
			}
			if (this.sendAICoreRequestReminder)
			{
				if (ResearchProjectTagDefOf.ShipRelated.CompletedProjects() < 2)
				{
					return;
				}
				if (PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.AIPersonaCore, 1) || PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.Ship_ComputerCore, 1))
				{
					return;
				}
				Faction faction = Find.FactionManager.RandomNonHostileFaction(false, false, true, TechLevel.Undefined);
				if (faction == null || faction.leader == null)
				{
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterLabelAICoreOffer".Translate(), "LetterAICoreOffer".Translate(faction.leader.LabelDefinite(), faction.NameColored, faction.leader.Named("PAWN")).Resolve().CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, faction, null, null, null, 0, true);
				this.sendAICoreRequestReminder = false;
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0004846A File Offset: 0x0004666A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x04000DE4 RID: 3556
		public bool sendAICoreRequestReminder = true;
	}
}

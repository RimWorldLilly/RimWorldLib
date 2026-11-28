using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001806 RID: 6150
	public class CompAncientSecurityTerminal : CompHackable
	{
		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x06009996 RID: 39318 RVA: 0x00378BF9 File Offset: 0x00376DF9
		private new CompProperties_AncientSecurityTerminalHackable Props
		{
			get
			{
				return (CompProperties_AncientSecurityTerminalHackable)this.props;
			}
		}

		// Token: 0x06009997 RID: 39319 RVA: 0x00378C08 File Offset: 0x00376E08
		protected override void OnHacked(Pawn hacker = null, bool suppressMessages = false)
		{
			base.OnHacked(hacker, suppressMessages);
			List<Building> list = this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.AncientBlastDoor).Concat(this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.Turret_AncientArmoredTurret)).Where(delegate(Building thing)
			{
				CompHackable compHackable;
				return thing.TryGetComp(out compHackable) && !compHackable.IsHacked;
			}).ToList<Building>();
			if (list.Empty<Building>())
			{
				if (hacker != null && !suppressMessages)
				{
					Messages.Message(this.Props.messageNoValidTarget.Formatted(hacker.Named("HACKER")), hacker, MessageTypeDefOf.NeutralEvent, true);
				}
				return;
			}
			Building building = list.RandomElementByWeight(delegate(Building x)
			{
				if (x.def != ThingDefOf.Turret_AncientArmoredTurret)
				{
					return 0.25f;
				}
				return 0.75f;
			});
			CompHackable comp = building.GetComp<CompHackable>();
			bool flag = false;
			if (comp.parent is Building_Turret && Rand.Chance(0.5f) && hacker != null)
			{
				flag = true;
				comp.parent.SetFaction(hacker.Faction, null);
				this.parent.Map.fogGrid.Unfog(comp.parent.Position);
			}
			else
			{
				comp.Hack(comp.defence, hacker, true);
			}
			if (hacker == null || suppressMessages)
			{
				return;
			}
			TaggedString label;
			TaggedString text;
			if (flag)
			{
				label = this.Props.letterTurretHackedLabel.Formatted(hacker.Named("HACKER"));
				text = this.Props.letterTurretHackedText.Formatted(hacker.Named("HACKER"));
			}
			else if (building.def == ThingDefOf.AncientBlastDoor)
			{
				label = this.Props.letterDoorOpenedLabel.Formatted(hacker.Named("HACKER"));
				text = this.Props.letterDoorOpenedText.Formatted(hacker.Named("HACKER"));
			}
			else
			{
				label = this.Props.letterTurretDisabledLabel.Formatted(hacker.Named("HACKER"));
				text = this.Props.letterTurretDisabledText.Formatted(hacker.Named("HACKER"));
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, building, null, null, null, null, 0, true);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200061A RID: 1562
	public class DeathLetter : ChoiceLetter
	{
		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x00138D34 File Offset: 0x00136F34
		protected DiaOption Option_ReadMore
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("ReadMore".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
					Find.LetterStack.RemoveLetter(this);
					InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
				};
				diaOption.resolveTree = true;
				if (!target.IsValid)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x00138DA2 File Offset: 0x00136FA2
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				DeathLetter.<get_Choices>d__3 <get_Choices>d__ = new DeathLetter.<get_Choices>d__3(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x00138DB4 File Offset: 0x00136FB4
		public override void OpenLetter()
		{
			Pawn targetPawn = this.lookTargets.TryGetPrimaryTarget().Thing as Pawn;
			TaggedString taggedString = base.Text;
			Func<LogEntry, bool> <>9__4;
			string text = (from entry in (from entry in (from battle in Find.BattleLog.Battles
			where battle.Concerns(targetPawn)
			select battle).SelectMany(delegate(Battle battle)
			{
				IEnumerable<LogEntry> entries = battle.Entries;
				Func<LogEntry, bool> predicate;
				if ((predicate = <>9__4) == null)
				{
					predicate = (<>9__4 = ((LogEntry entry) => entry.Concerns(targetPawn) && entry.ShowInCompactView()));
				}
				return entries.Where(predicate);
			})
			orderby entry.Age
			select entry).Take(5).Reverse<LogEntry>()
			select "  " + entry.ToGameStringFromPOV(null, false)).ToLineList(null, false);
			if (text.Length > 0)
			{
				taggedString = string.Format("{0}\n\n{1}\n{2}", taggedString, "LastEventsInLife".Translate(targetPawn.LabelDefinite(), targetPawn.Named("PAWN")).Resolve() + ":", text);
			}
			DiaNode diaNode = new DiaNode(taggedString);
			diaNode.options.AddRange(this.Choices);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title));
		}
	}
}

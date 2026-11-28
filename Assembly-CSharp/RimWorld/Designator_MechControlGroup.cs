using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4F RID: 7247
	public class Designator_MechControlGroup : Designator
	{
		// Token: 0x1700215D RID: 8541
		// (get) Token: 0x0600B8C9 RID: 47305 RVA: 0x00412C38 File Offset: 0x00410E38
		public static IEnumerable<MechanitorControlGroup> SelectedControlGroups
		{
			get
			{
				return new Designator_MechControlGroup.<get_SelectedControlGroups>d__2(-2);
			}
		}

		// Token: 0x0600B8CA RID: 47306 RVA: 0x00412C44 File Offset: 0x00410E44
		public Designator_MechControlGroup()
		{
			this.defaultLabel = "DesignatorMechControlGroup".Translate() + "...";
			this.defaultDesc = "DesignatorMechControlGroupDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/MechControlGroup", true);
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0600B8CB RID: 47307 RVA: 0x00412CA4 File Offset: 0x00410EA4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B8CC RID: 47308 RVA: 0x00412D30 File Offset: 0x00410F30
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null || !pawn.IsColonyMech || pawn.GetOverseer() == null)
			{
				return false;
			}
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in Find.Selector.SelectedPawns)
			{
				Pawn overseer = pawn3.GetOverseer();
				if (pawn2 != null && pawn2 != overseer)
				{
					return false;
				}
				pawn2 = overseer;
			}
			AcceptanceReport canControlMechs = pawn2.mechanitor.CanControlMechs;
			if (canControlMechs)
			{
				return true;
			}
			if (!canControlMechs.Reason.NullOrEmpty())
			{
				return ("DisabledCommand".Translate() + ": " + canControlMechs.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return false;
		}

		// Token: 0x0600B8CD RID: 47309 RVA: 0x00412E20 File Offset: 0x00411020
		public override void DesignateThing(Thing t)
		{
			this.ProcessInput(null);
		}

		// Token: 0x0600B8CE RID: 47310 RVA: 0x00412E2C File Offset: 0x0041102C
		private bool CanAssignToControlGroup(Pawn pawn, MechanitorControlGroup group)
		{
			Pawn overseer = pawn.GetOverseer();
			return overseer != null && group.Tracker.Pawn == overseer;
		}

		// Token: 0x0600B8CF RID: 47311 RVA: 0x00412E54 File Offset: 0x00411054
		public override void ProcessInput(Event ev)
		{
			Designator_MechControlGroup.tmpSelectedControlGroups.Clear();
			Designator_MechControlGroup.tmpSelectedControlGroups.AddRange(Designator_MechControlGroup.SelectedControlGroups);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < Designator_MechControlGroup.tmpSelectedControlGroups.Count; i++)
			{
				MechanitorControlGroup selected = Designator_MechControlGroup.tmpSelectedControlGroups[i];
				if (Find.Selector.SelectedPawns.All((Pawn p) => p.GetMechControlGroup() == selected))
				{
					FloatMenuOption item = new FloatMenuOption("CannotAssignMechToControlGroup".Translate(selected.Index) + " (" + selected.WorkMode.LabelCap.ToString() + ")" + ": " + "AssignMechAlreadyAssigned".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
				else
				{
					FloatMenuOption item2 = new FloatMenuOption("AssignMechToControlGroup".Translate(selected.Index) + " (" + selected.WorkMode.LabelCap.ToString() + ")", delegate()
					{
						foreach (Pawn pawn in Find.Selector.SelectedPawns)
						{
							if (this.CanAssignToControlGroup(pawn, selected))
							{
								selected.Assign(pawn);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item2);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
			Designator_MechControlGroup.tmpSelectedControlGroups.Clear();
		}

		// Token: 0x0600B8D0 RID: 47312 RVA: 0x00412FFD File Offset: 0x004111FD
		public override void SelectedUpdate()
		{
		}

		// Token: 0x040069B5 RID: 27061
		private static readonly HashSet<MechanitorControlGroup> tmpControlGroups = new HashSet<MechanitorControlGroup>();

		// Token: 0x040069B6 RID: 27062
		private static List<MechanitorControlGroup> tmpSelectedControlGroups = new List<MechanitorControlGroup>();
	}
}

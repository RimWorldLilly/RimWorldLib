using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D27 RID: 7463
	public class FloatMenuOptionProvider_DropEquipment : FloatMenuOptionProvider
	{
		// Token: 0x17002356 RID: 9046
		// (get) Token: 0x0600C124 RID: 49444 RVA: 0x0046673F File Offset: 0x0046493F
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002357 RID: 9047
		// (get) Token: 0x0600C125 RID: 49445 RVA: 0x00466742 File Offset: 0x00464942
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002358 RID: 9048
		// (get) Token: 0x0600C126 RID: 49446 RVA: 0x00466745 File Offset: 0x00464945
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002359 RID: 9049
		// (get) Token: 0x0600C127 RID: 49447 RVA: 0x00466748 File Offset: 0x00464948
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C128 RID: 49448 RVA: 0x0046674B File Offset: 0x0046494B
		protected override bool AppliesInt(FloatMenuContext context)
		{
			Pawn_EquipmentTracker equipment = context.FirstSelectedPawn.equipment;
			return ((equipment != null) ? equipment.Primary : null) != null;
		}

		// Token: 0x0600C129 RID: 49449 RVA: 0x00466768 File Offset: 0x00464968
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn != context.FirstSelectedPawn)
			{
				return null;
			}
			if (clickedPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanUnequip(clickedPawn.equipment.Primary, clickedPawn))
			{
				return new FloatMenuOption("CannotDrop".Translate(clickedPawn.equipment.Primary.Label, clickedPawn.equipment.Primary) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action action = delegate()
			{
				clickedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, clickedPawn.equipment.Primary), new JobTag?(JobTag.Misc), false);
			};
			return new FloatMenuOption("Drop".Translate(clickedPawn.equipment.Primary.Label, clickedPawn.equipment.Primary), action, clickedPawn.equipment.Primary, Color.white, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0);
		}
	}
}

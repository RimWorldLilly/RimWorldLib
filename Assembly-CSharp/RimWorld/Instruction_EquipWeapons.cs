using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0C RID: 7692
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		// Token: 0x170024E6 RID: 9446
		// (get) Token: 0x0600C7E4 RID: 51172 RVA: 0x00495AD4 File Offset: 0x00493CD4
		protected override float ProgressPercent
		{
			get
			{
				IEnumerable<Pawn> source = from c in base.Map.mapPawns.FreeColonists
				where !LifeStageUtility.AlwaysDowned(c)
				select c;
				return (float)(from c in source
				where c.equipment.Primary != null
				select c).Count<Pawn>() / (float)source.Count<Pawn>();
			}
		}

		// Token: 0x170024E7 RID: 9447
		// (get) Token: 0x0600C7E5 RID: 51173 RVA: 0x00495B49 File Offset: 0x00493D49
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x0600C7E6 RID: 51174 RVA: 0x00495B79 File Offset: 0x00493D79
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x0600C7E7 RID: 51175 RVA: 0x00495B9C File Offset: 0x00493D9C
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600C7E8 RID: 51176 RVA: 0x00495BF8 File Offset: 0x00493DF8
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.Weapons)
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

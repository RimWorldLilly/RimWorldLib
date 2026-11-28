using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0A RID: 7690
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x170024E5 RID: 9445
		// (get) Token: 0x0600C7DB RID: 51163 RVA: 0x00495768 File Offset: 0x00493968
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x0600C7DC RID: 51164 RVA: 0x004957C1 File Offset: 0x004939C1
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

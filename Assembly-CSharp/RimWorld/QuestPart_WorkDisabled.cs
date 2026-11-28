using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001092 RID: 4242
	public class QuestPart_WorkDisabled : QuestPartActivable
	{
		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06006A48 RID: 27208 RVA: 0x0025633D File Offset: 0x0025453D
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				QuestPart_WorkDisabled.<get_DisabledWorkTypes>d__3 <get_DisabledWorkTypes>d__ = new QuestPart_WorkDisabled.<get_DisabledWorkTypes>d__3(-2);
				<get_DisabledWorkTypes>d__.<>4__this = this;
				return <get_DisabledWorkTypes>d__;
			}
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x0025634D File Offset: 0x0025454D
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x06006A4A RID: 27210 RVA: 0x0025635C File Offset: 0x0025455C
		public override void Cleanup()
		{
			base.Cleanup();
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x06006A4B RID: 27211 RVA: 0x0025636C File Offset: 0x0025456C
		private void ClearPawnWorkTypesAndSkillsCache()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i] != null)
				{
					this.pawns[i].Notify_DisabledWorkTypesChanged();
					if (this.pawns[i].skills != null)
					{
						this.pawns[i].skills.Notify_SkillDisablesChanged();
					}
				}
			}
		}

		// Token: 0x06006A4C RID: 27212 RVA: 0x002563D8 File Offset: 0x002545D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<WorkTags>(ref this.disabledWorkTags, "disabledWorkTags", WorkTags.None, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400437F RID: 17279
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004380 RID: 17280
		public WorkTags disabledWorkTags;
	}
}

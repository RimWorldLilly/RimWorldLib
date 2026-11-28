using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D0 RID: 6096
	public class TaleData_Def : TaleData
	{
		// Token: 0x0600981E RID: 38942 RVA: 0x00372690 File Offset: 0x00370890
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = ((this.def != null) ? this.def.defName : null);
				this.tmpDefType = ((this.def != null) ? this.def.GetType() : null);
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.tmpDefName != null)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, BackCompatibility.BackCompatibleDefName(this.tmpDefType, this.tmpDefName, false, null), true);
			}
		}

		// Token: 0x0600981F RID: 38943 RVA: 0x00372736 File Offset: 0x00370936
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			TaleData_Def.<GetRules>d__4 <GetRules>d__ = new TaleData_Def.<GetRules>d__4(-2);
			<GetRules>d__.<>4__this = this;
			<GetRules>d__.<>3__prefix = prefix;
			return <GetRules>d__;
		}

		// Token: 0x06009820 RID: 38944 RVA: 0x0037274D File Offset: 0x0037094D
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		// Token: 0x04005AF9 RID: 23289
		public Def def;

		// Token: 0x04005AFA RID: 23290
		private string tmpDefName;

		// Token: 0x04005AFB RID: 23291
		private Type tmpDefType;
	}
}

using System;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x02000675 RID: 1653
	public class Dialog_RenamePolicy : Dialog_Rename<Policy>
	{
		// Token: 0x06003426 RID: 13350 RVA: 0x00150518 File Offset: 0x0014E718
		public Dialog_RenamePolicy(Policy policy) : base(policy)
		{
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x00150524 File Offset: 0x0014E724
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (!Dialog_RenamePolicy.ValidNameRegex.IsMatch(name))
			{
				return "InvalidName".Translate();
			}
			return true;
		}

		// Token: 0x04002214 RID: 8724
		private static readonly Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}

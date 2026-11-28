using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006FE RID: 1790
	public class KeyBindingData
	{
		// Token: 0x060038CD RID: 14541 RVA: 0x0016CA74 File Offset: 0x0016AC74
		public KeyBindingData()
		{
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0016CA7C File Offset: 0x0016AC7C
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x0016CA94 File Offset: 0x0016AC94
		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != KeyCode.None)
			{
				str += this.keyBindingA.ToString();
			}
			if (this.keyBindingB != KeyCode.None)
			{
				str = str + ", " + this.keyBindingB.ToString();
			}
			return str + "]";
		}

		// Token: 0x040024F9 RID: 9465
		public KeyCode keyBindingA;

		// Token: 0x040024FA RID: 9466
		public KeyCode keyBindingB;
	}
}

using System;

namespace Verse.Sound
{
	// Token: 0x020007CC RID: 1996
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003FC6 RID: 16326 RVA: 0x00197DDD File Offset: 0x00195FDD
		public override string Label
		{
			get
			{
				if (this.inParamName == "")
				{
					return "Undefined external";
				}
				return this.inParamName;
			}
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x00197E00 File Offset: 0x00196000
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out result))
			{
				return result;
			}
			return this.defaultValue;
		}

		// Token: 0x04002A4D RID: 10829
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x04002A4E RID: 10830
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}

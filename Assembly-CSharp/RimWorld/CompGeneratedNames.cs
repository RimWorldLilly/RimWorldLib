using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020018A2 RID: 6306
	public class CompGeneratedNames : ThingComp
	{
		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x06009DF2 RID: 40434 RVA: 0x0038B60F File Offset: 0x0038980F
		public CompProperties_GeneratedName Props
		{
			get
			{
				return (CompProperties_GeneratedName)this.props;
			}
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x06009DF3 RID: 40435 RVA: 0x0038B61C File Offset: 0x0038981C
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06009DF4 RID: 40436 RVA: 0x0038B624 File Offset: 0x00389824
		public static string GenerateName(CompProperties_GeneratedName props)
		{
			return GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_weapon_name", new GrammarRequest
			{
				Includes = 
				{
					props.nameMaker
				}
			}, null, false, null, null, null, true));
		}

		// Token: 0x06009DF5 RID: 40437 RVA: 0x0038B664 File Offset: 0x00389864
		public override string TransformLabel(string label)
		{
			if (this.parent.StyleSourcePrecept != null)
			{
				return label;
			}
			if (this.parent.GetComp<CompBladelinkWeapon>() != null)
			{
				return this.name + ", " + label;
			}
			return this.name + " (" + label + ")";
		}

		// Token: 0x06009DF6 RID: 40438 RVA: 0x0038B6B5 File Offset: 0x003898B5
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.name = CompGeneratedNames.GenerateName(this.Props);
		}

		// Token: 0x06009DF7 RID: 40439 RVA: 0x0038B6CF File Offset: 0x003898CF
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
		}

		// Token: 0x04005D73 RID: 23923
		private string name;
	}
}

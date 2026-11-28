using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FED RID: 4077
	public class StorytellerCompProperties
	{
		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06006714 RID: 26388 RVA: 0x00248994 File Offset: 0x00246B94
		public bool Enabled
		{
			get
			{
				if (!this.enableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int i = 0; i < this.enableIfAnyModActive.Count; i++)
					{
						if (ModsConfig.IsActive(this.enableIfAnyModActive[i]))
						{
							return true;
						}
					}
					return false;
				}
				if (!this.disableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int j = 0; j < this.disableIfAnyModActive.Count; j++)
					{
						if (ModsConfig.IsActive(this.disableIfAnyModActive[j]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06006715 RID: 26389 RVA: 0x00248A14 File Offset: 0x00246C14
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06006716 RID: 26390 RVA: 0x00248A27 File Offset: 0x00246C27
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06006717 RID: 26391 RVA: 0x00248A41 File Offset: 0x00246C41
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			StorytellerCompProperties.<ConfigErrors>d__11 <ConfigErrors>d__ = new StorytellerCompProperties.<ConfigErrors>d__11(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006718 RID: 26392 RVA: 0x00248A51 File Offset: 0x00246C51
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		// Token: 0x040040BB RID: 16571
		[TranslationHandle]
		public Type compClass;

		// Token: 0x040040BC RID: 16572
		public float minDaysPassed;

		// Token: 0x040040BD RID: 16573
		public List<IncidentTargetTagDef> allowedTargetTags;

		// Token: 0x040040BE RID: 16574
		public List<IncidentTargetTagDef> disallowedTargetTags;

		// Token: 0x040040BF RID: 16575
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x040040C0 RID: 16576
		public List<string> enableIfAnyModActive;

		// Token: 0x040040C1 RID: 16577
		public List<string> disableIfAnyModActive;
	}
}

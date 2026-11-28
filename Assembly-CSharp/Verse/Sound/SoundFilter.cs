using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C4 RID: 1988
	public abstract class SoundFilter
	{
		// Token: 0x06003FB6 RID: 16310
		public abstract void SetupOn(AudioSource source);

		// Token: 0x06003FB7 RID: 16311 RVA: 0x00197BD8 File Offset: 0x00195DD8
		protected static T GetOrMakeFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T t = source.gameObject.GetComponent<T>();
			if (t != null)
			{
				t.enabled = true;
			}
			else
			{
				t = source.gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}

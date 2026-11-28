using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200068C RID: 1676
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060034FB RID: 13563 RVA: 0x00158DBA File Offset: 0x00156FBA
		private void Start()
		{
			if (Screen.width != 0 && Screen.height != 0)
			{
				Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
			}
		}
	}
}

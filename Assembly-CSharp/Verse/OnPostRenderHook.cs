using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EE RID: 494
	public static class OnPostRenderHook
	{
		// Token: 0x06000DF7 RID: 3575 RVA: 0x00048484 File Offset: 0x00046684
		static OnPostRenderHook()
		{
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(OnPostRenderHook.OnPreRender));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(OnPostRenderHook.OnPostRender));
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000484DC File Offset: 0x000466DC
		public static void HookOnce(Camera camera, Action postRender)
		{
			OnPostRenderHook.hooks.Add(camera, new OnPostRenderHook.Callbacks
			{
				postRender = postRender,
				preRenderCalled = false
			});
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00048510 File Offset: 0x00046710
		private static void OnPreRender(Camera camera)
		{
			OnPostRenderHook.Callbacks callbacks;
			if (OnPostRenderHook.hooks.TryGetValue(camera, out callbacks))
			{
				OnPostRenderHook.hooks[camera] = new OnPostRenderHook.Callbacks
				{
					postRender = callbacks.postRender,
					preRenderCalled = true
				};
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00048558 File Offset: 0x00046758
		private static void OnPostRender(Camera camera)
		{
			OnPostRenderHook.Callbacks callbacks;
			if (OnPostRenderHook.hooks.TryGetValue(camera, out callbacks) && callbacks.preRenderCalled)
			{
				OnPostRenderHook.hooks.Remove(camera);
				callbacks.postRender();
			}
		}

		// Token: 0x04000DE5 RID: 3557
		private static Dictionary<Camera, OnPostRenderHook.Callbacks> hooks = new Dictionary<Camera, OnPostRenderHook.Callbacks>();

		// Token: 0x02002581 RID: 9601
		private struct Callbacks
		{
			// Token: 0x040099E5 RID: 39397
			public bool preRenderCalled;

			// Token: 0x040099E6 RID: 39398
			public Action postRender;
		}
	}
}

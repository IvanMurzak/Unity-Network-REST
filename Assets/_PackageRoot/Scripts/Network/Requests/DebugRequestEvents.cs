using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace Network.Extension
{
	public class DebugRequestEvents<T> : RequestEvents<T>
	{
		public DebugRequestEvents(CompositeDisposable compositeDisposable, Request<T> request, int id) : base()
		{
	#if UNITY_EDITOR
			OnSuccess				.Subscribe(x => Debug.Log		($"[{request.RESTMethod}] {id} - OnSuccess\n\n{JsonConvert.SerializeObject(x, Formatting.Indented)}\n"))		.AddTo(compositeDisposable);
			OnSuccessRaw			.Subscribe(x => Debug.Log		($"[{request.RESTMethod}] {id} - OnSuccessRaw\n\n{Request<T>.JsonPrettify(x)}\n"))								.AddTo(compositeDisposable);
	#endif
			OnSerializationError	.Subscribe(x => Debug.LogError	($"[{request.RESTMethod}] {id} - OnSerializationError: {x}")).AddTo(compositeDisposable);
			OnHttpError				.Subscribe(x => Debug.LogError	($"[{request.RESTMethod}] {id} - OnHttpError\n\n {JsonConvert.SerializeObject(x, Formatting.Indented)}\n"))		.AddTo(compositeDisposable);
			OnHttpErrorRaw			.Subscribe(x => Debug.LogError	($"[{request.RESTMethod}] {id} - OnHttpErrorRaw\n\n{x}\n"))														.AddTo(compositeDisposable);
			OnNetworkError			.Subscribe(x => Debug.LogError	($"[{request.RESTMethod}] {id} - OnNetworkError\n\n {JsonConvert.SerializeObject(x, Formatting.Indented)}\n"))	.AddTo(compositeDisposable);
		}
	}
}
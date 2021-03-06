#pragma once

#include "CaptureManagerTypeInfo.h"
#include "BaseDispatch.h"
#include "../CaptureManagerBroker/StreamControlCommon.h"
#include "../PugiXML/pugixml.hpp"


namespace CaptureManager
{
	namespace COMServer
	{
		class MixerNodeFactory :
			public BaseDispatch<IMixerNodeFactory>
		{
		public:
			MixerNodeFactory();
			virtual ~MixerNodeFactory();


			static StreamControlInfo getStreamControlInfo(
				pugi::xml_node& aRefRootXMLElement,
				std::vector<StreamControlInfo>& aCollection);

			// IMixerNodeFactory DWORDerface

			STDMETHOD(createMixerNodes)(
				/* [in] */ IUnknown *aPtrDownStreamTopologyNode,
				/* [in] */ DWORD aInputNodeAmount,
				/* [out] */ VARIANT *aPtrArrayPtrTopologyInputNodes);


			// IDispatch interface stub

			STDMETHOD(GetIDsOfNames)(
				__RPC__in REFIID riid,
				/* [size_is][in] */ __RPC__in_ecount_full(cNames) LPOLESTR *rgszNames,
				/* [range][in] */ __RPC__in_range(0, 16384) UINT cNames,
				LCID lcid,
				/* [size_is][out] */ __RPC__out_ecount_full(cNames) DISPID *rgDispId);

			virtual HRESULT invokeMethod(
				/* [annotation][in] */
				_In_  DISPID dispIdMember,
				/* [annotation][out][in] */
				_In_  DISPPARAMS *pDispParams,
				/* [annotation][out] */
				_Out_opt_  VARIANT *pVarResult);
		};
	}
}
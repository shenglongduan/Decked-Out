﻿#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Void Unity.Services.Core.Telemetry.Internal.Diagnostics::SendDiagnostic(System.String,System.String,System.Collections.Generic.IDictionary`2<System.String,System.String>)
extern void Diagnostics_SendDiagnostic_mF569E18F0662E882CD37D4BC894BC085CB222D08 (void);
// 0x00000002 System.Void Unity.Services.Core.Telemetry.Internal.Diagnostics::.ctor()
extern void Diagnostics__ctor_m2CF108329965DFBA2D13399A65E9D5D47BF63BD8 (void);
// 0x00000003 System.Collections.Generic.IReadOnlyDictionary`2<System.String,System.String> Unity.Services.Core.Telemetry.Internal.DiagnosticsFactory::get_CommonTags()
extern void DiagnosticsFactory_get_CommonTags_mA08B1FB6FCB5AB504EF2ED3C2ED27EF41C442B7C (void);
// 0x00000004 Unity.Services.Core.Telemetry.Internal.IDiagnostics Unity.Services.Core.Telemetry.Internal.DiagnosticsFactory::Create(System.String)
extern void DiagnosticsFactory_Create_mE152323E55C07B54470CB90071252FF5F24A138B (void);
// 0x00000005 System.Void Unity.Services.Core.Telemetry.Internal.DiagnosticsFactory::.ctor()
extern void DiagnosticsFactory__ctor_m6AFD4725FFA4F4054B13917A376B8BE059AF4D27 (void);
// 0x00000006 System.Void Unity.Services.Core.Telemetry.Internal.Metrics::Unity.Services.Core.Telemetry.Internal.IMetrics.SendGaugeMetric(System.String,System.Double,System.Collections.Generic.IDictionary`2<System.String,System.String>)
extern void Metrics_Unity_Services_Core_Telemetry_Internal_IMetrics_SendGaugeMetric_m44A74F915A2C4302A6BF07BE010D16D9793D7BE6 (void);
// 0x00000007 System.Void Unity.Services.Core.Telemetry.Internal.Metrics::Unity.Services.Core.Telemetry.Internal.IMetrics.SendHistogramMetric(System.String,System.Double,System.Collections.Generic.IDictionary`2<System.String,System.String>)
extern void Metrics_Unity_Services_Core_Telemetry_Internal_IMetrics_SendHistogramMetric_mA7EB1B96D0F7E39A951A68AF58BF6A455E27C9ED (void);
// 0x00000008 System.Void Unity.Services.Core.Telemetry.Internal.Metrics::Unity.Services.Core.Telemetry.Internal.IMetrics.SendSumMetric(System.String,System.Double,System.Collections.Generic.IDictionary`2<System.String,System.String>)
extern void Metrics_Unity_Services_Core_Telemetry_Internal_IMetrics_SendSumMetric_m3698244EA67D8736E51D84935E05E2DEB290C6A8 (void);
// 0x00000009 System.Void Unity.Services.Core.Telemetry.Internal.Metrics::.ctor()
extern void Metrics__ctor_m6318435D0C0C0AD81ABC2B7D542FE92D7B5C7D79 (void);
// 0x0000000A System.Collections.Generic.IReadOnlyDictionary`2<System.String,System.String> Unity.Services.Core.Telemetry.Internal.MetricsFactory::get_CommonTags()
extern void MetricsFactory_get_CommonTags_m8B0B37504BB0C83E60181E6C273B5710E595B37E (void);
// 0x0000000B Unity.Services.Core.Telemetry.Internal.IMetrics Unity.Services.Core.Telemetry.Internal.MetricsFactory::Create(System.String)
extern void MetricsFactory_Create_m6DCE478FE70DE3D0533A150550812CE038D8CD46 (void);
// 0x0000000C System.Void Unity.Services.Core.Telemetry.Internal.MetricsFactory::.ctor()
extern void MetricsFactory__ctor_mD15F90B2C781DE2B48F41556FECEFAB2347AFC0C (void);
static Il2CppMethodPointer s_methodPointers[12] = 
{
	Diagnostics_SendDiagnostic_mF569E18F0662E882CD37D4BC894BC085CB222D08,
	Diagnostics__ctor_m2CF108329965DFBA2D13399A65E9D5D47BF63BD8,
	DiagnosticsFactory_get_CommonTags_mA08B1FB6FCB5AB504EF2ED3C2ED27EF41C442B7C,
	DiagnosticsFactory_Create_mE152323E55C07B54470CB90071252FF5F24A138B,
	DiagnosticsFactory__ctor_m6AFD4725FFA4F4054B13917A376B8BE059AF4D27,
	Metrics_Unity_Services_Core_Telemetry_Internal_IMetrics_SendGaugeMetric_m44A74F915A2C4302A6BF07BE010D16D9793D7BE6,
	Metrics_Unity_Services_Core_Telemetry_Internal_IMetrics_SendHistogramMetric_mA7EB1B96D0F7E39A951A68AF58BF6A455E27C9ED,
	Metrics_Unity_Services_Core_Telemetry_Internal_IMetrics_SendSumMetric_m3698244EA67D8736E51D84935E05E2DEB290C6A8,
	Metrics__ctor_m6318435D0C0C0AD81ABC2B7D542FE92D7B5C7D79,
	MetricsFactory_get_CommonTags_m8B0B37504BB0C83E60181E6C273B5710E595B37E,
	MetricsFactory_Create_m6DCE478FE70DE3D0533A150550812CE038D8CD46,
	MetricsFactory__ctor_mD15F90B2C781DE2B48F41556FECEFAB2347AFC0C,
};
static const int32_t s_InvokerIndices[12] = 
{
	1306,
	4964,
	4848,
	3584,
	4964,
	1275,
	1275,
	1275,
	4964,
	4848,
	3584,
	4964,
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_Unity_Services_Core_Telemetry_CodeGenModule;
const Il2CppCodeGenModule g_Unity_Services_Core_Telemetry_CodeGenModule = 
{
	"Unity.Services.Core.Telemetry.dll",
	12,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};

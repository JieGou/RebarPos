#define WIN32_LEAN_AND_MEAN
#if defined(_DEBUG) && !defined(AC_FULL_DEBUG)
#error _DEBUG should not be defined except in internal Adesk debug builds
#endif

#include <windows.h>
#include <objbase.h>

#include "rxregsvc.h"

#include "assert.h"
#include "math.h"

#include "gepnt3d.h"
#include "gevec3d.h"
#include "gelnsg3d.h"
#include "gearc3d.h"

#include "dbents.h"
#include "dbsymtb.h"
#include "dbcfilrs.h"
#include "dbspline.h"
#include "dbproxy.h"
#include "dbxutil.h"
#include "acutmem.h"

#include "Utility.h"
#include "RebarPos.h"

#include "acdb.h"
#include "dbidmap.h"
#include "adesk.h"

#include "dbapserv.h"
#include "appinfo.h"
#include "tchar.h"


extern "C" {

// locally defined entry point invoked by Rx.

AcRx::AppRetCode __declspec(dllexport) acrxEntryPoint(AcRx::AppMsgCode msg, void* pkt);

}

void changeAppNameCallback(const AcRxClass* classObj, TCHAR*& newAppName, int saveVer)
{
    if (saveVer == AcDb::kDHL_1014 && classObj == CRebarPos::desc())
        acutNewString(_T("RebarPos")
        _T("|Product Desc:     RebarPos ARX App")
        _T("|Company:          OZOZ"), newAppName);
}

AcRx::AppRetCode __declspec(dllexport)
acrxEntryPoint(AcRx::AppMsgCode msg, void* pkt)
{
    switch(msg) 
	{
    case AcRx::kInitAppMsg:
        acrxUnlockApplication(pkt);     // Try to allow unloading
        acrxRegisterAppMDIAware(pkt);
        CRebarPos::rxInit(changeAppNameCallback);
        
        // Register a service using the class name.
        if (!acrxServiceIsRegistered(_T("CRebarPos")))
            acrxRegisterService(_T("CRebarPos"));

        acrxBuildClassHierarchy();
        break;

    case AcRx::kUnloadAppMsg:
        // Unregister the service
        AcRxObject *obj = acrxServiceDictionary->remove(_T("CRebarPos"));
        if (obj != NULL)
            delete obj;

        deleteAcRxClass(CRebarPos::desc());
        break;
    }
    return AcRx::kRetOK;
}
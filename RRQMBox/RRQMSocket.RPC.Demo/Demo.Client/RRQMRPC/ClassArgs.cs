using System;
using RRQMSocket.RPC;
using RRQMCore.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
namespace RRQMRPC.RRQMTest
{

public class Test01
{
public System.Int32 Age{get;set;}
public System.String Name{get;set;}
}


public class Test02
{
public System.Int32 Age{get;set;}
public System.String Name{get;set;}
public List<System.Int32> list{get;set;}
public System.Int32[] nums{get;set;}
}


public class Test03
: RRQMRPC.RRQMTest.Test02
{
public System.Int32 Length{get;set;}
}

}

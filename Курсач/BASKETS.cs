
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Курсач
{

using System;
    using System.Collections.Generic;
    
public partial class BASKETS
{

    public int BASKET_ID { get; set; }

    public Nullable<int> USER_ID { get; set; }

    public Nullable<int> BOOK_ID { get; set; }



    public virtual BOOKS BOOKS { get; set; }

    public virtual USERS USERS { get; set; }

}

}

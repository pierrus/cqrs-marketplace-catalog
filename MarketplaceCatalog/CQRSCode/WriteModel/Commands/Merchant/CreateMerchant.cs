﻿using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class CreateMerchant : ICommand 
	{
        public readonly String Name;

        public readonly String Email;

        public readonly UInt16 Commission;

        public CreateMerchant(Guid id, String name, String email, UInt16 commission) 
		{
            Id = id;
            Name = name;
            Email = email;
            Commission = commission;
        }

        public Guid Id { get; set; }
        
        public int ExpectedVersion { get; set; }
	}
}
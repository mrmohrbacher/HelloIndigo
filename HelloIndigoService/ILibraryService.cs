﻿using System;
using System.IO;
using System.ServiceModel;

using LibraryModel;

namespace HelloIndigo
   {
   [ServiceContract(Namespace = "uri://blackriverinc.com/helloindigo/LibraryService")]
   public interface ILibraryService
      {
      [OperationContract]
      bool List(string searchKey, out Book[] books);

      [OperationContract]
      bool Read(string key, out Book book);

      [OperationContract]
      bool Update(ref Book book);

      [OperationContract]
      bool Add(Book book);

      [OperationContract]
      bool Delete(string key, DateTime timeStamp);

		[OperationContract]
		bool Load(Stream input);
      }
   }

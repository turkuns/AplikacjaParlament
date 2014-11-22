﻿//
//  JsonObjectRequestHandler.cs
//
//  Author:
//       Jakub Syty <j.syty@media30.pl>
//
//  Copyright (c) 2014 
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AplikacjaParlamentShared.Api
{
	public class JsonObjectRequestHandler<T> : IJsonObjectRequestHandler<T>
	{
		private IConnectionProvider ConnectionProvider; 

		public JsonObjectRequestHandler (IConnectionProvider connectionProvider)
		{
			this.ConnectionProvider = connectionProvider;
		}

		public async Task<T> GetJsonObjectAsync (string uri)
		{
			var response = await ConnectionProvider.GetHttpClient().GetAsync(uri);

			response.EnsureSuccessStatusCode();

			string content = await response.Content.ReadAsStringAsync();
			JToken obj;
			bool hasValue = JObject.Parse (content).TryGetValue ("object", out obj);
			if (!hasValue) //jeżeli nie istnieje element object w odpowiedzi json
				throw new NoObjectJsonElementException ();
			if(obj.Type == JTokenType.Boolean) //jeżeli element object z odpowiedzi json ma typ boolean (nie muszę sprawdzać wartości, wystarczy wiedza na temat typu)
				throw new ObjectJsonBooleanElementException();
			return DataObjectParser.ParseJObjectToType<T> (obj as JObject);
		}
	}
}

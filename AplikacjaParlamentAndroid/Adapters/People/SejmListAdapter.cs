﻿//
//  SejmListAdapter.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using com.refractored.monodroidtoolkit.imageloader;
using AplikacjaParlamentShared.Models;

namespace AplikacjaParlamentAndroid.Adapters
{

	public class SejmListAdapter : BaseAdapter<Posel>
	{

		private class Wrapper : Java.Lang.Object
		{
			public TextView ImieNazwisko { get; set; }
			public TextView Partia { get; set; }
			public ImageView Miniature { get; set; }
			public TextView Okreg { get; set; }
		}

		private Activity context;
		private List<Posel> list; 

		public SejmListAdapter(Activity context, List<Posel> list)
		{
			this.context = context;
			this.list = list;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			Wrapper wrapper = null;
			var view = convertView;
			if (convertView == null)
			{
				view = context.LayoutInflater.Inflate(Resource.Layout.SejmListElement, null);
				wrapper = new Wrapper();
				wrapper.ImieNazwisko = view.FindViewById<TextView>(Resource.Id.imieNazwisko);
				wrapper.Partia = view.FindViewById<TextView>(Resource.Id.partia);
				wrapper.Miniature = view.FindViewById<ImageView> (Resource.Id.miniature);
				wrapper.Okreg = view.FindViewById<TextView> (Resource.Id.okreg);
				view.Tag = wrapper;
			}
			else
			{
				wrapper = convertView.Tag as Wrapper;
			}

			var posel = list[position];
			wrapper.ImieNazwisko.Text = String.Concat(posel.Imie, " ", posel.Nazwisko);
			wrapper.Partia.Text = posel.SejmKlubyNazwa;
			wrapper.Okreg.Text = posel.OkregWyborczyNumer.ToString();
			loadImage (wrapper, String.Concat ("http://resources.sejmometr.pl/mowcy/a/0/", posel.MowcaId, ".jpg"));

			return view;
		}

		async private void loadImage(Wrapper wrapper, string url){
			await ImagesHelper.SetImageFromUrlAsync(wrapper.Miniature,url, context);
		}

		public override Posel this[int position]
		{
			get { return list[position]; }
		}

		public override int Count
		{
			get { return list.Count; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}
	}
}
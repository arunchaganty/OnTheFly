/*
 * OnTheFly - Tomboy Addin
 * Arun Chaganty <arunchaganty@gmail.com>
 * Allows on the fly wikitization, e.g. *something* -> <bold>something</bold>
 *
 */

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

//using Mono.Unix;

using Tomboy;

namespace Tomboy.OnTheFly
{
	public class OnTheFlyNoteAddin : NoteAddin
	{
		NoteTag bold_tag;
		NoteTag italic_tag;
		NoteTag underline_tag;
		NoteTag strikethrough_tag;
		NoteTag highlight_tag;

		/* TODO: Support for autolinks 
		NoteTag url_tag;
		NoteTag link_tag;
		NoteTag broken_link_tag;
		*/

		const string bold_id = "*";
		const string italic_id = "/";
		const string underlined_id = "_";
		const string strikethrough_id = "+";
		const string highlight_id = "=";

		
		public override void Initialize ()
		{
			bold_tag = (NoteTag) Note.TagTable.Lookup ("bold");
			italic_tag = (NoteTag) Note.TagTable.Lookup ("italic");
			highlight_tag = (NoteTag) Note.TagTable.Lookup ("highlight");
			underline_tag = (NoteTag) Note.TagTable.Lookup ("underline");
			strikethrough_tag = (NoteTag) Note.TagTable.Lookup ("strikethrough");

			/*
			url_tag = (NoteTag) Note.TagTable.Lookup ("link:url");
			link_tag = (NoteTag) Note.TagTable.Lookup ("link:internal");
			broken_link_tag = (NoteTag) Note.TagTable.Lookup ("link:broken");
			*/
		}

		public override void Shutdown ()
		{
		}

		static OnTheFlyNoteAddin ()
		{
		}

		public override void OnNoteOpened ()
		{
			Buffer.InsertText += OnInsertText;
		}

		void OnInsertText (object sender, Gtk.InsertTextArgs args)
		{
			Gtk.TextIter start;
			Gtk.TextIter end = args.Pos;
			end.BackwardChars(2);
			Gtk.TextIter iter = end;
			string id;
			switch (end.Char) {
				case bold_id:
					id = bold_id;
					break;
				case italic_id:
					id = italic_id;
					break;
				case underlined_id:
					id = underlined_id;
					break;
				case highlight_id:
					id = highlight_id;
					break;
				case strikethrough_id:
					id = strikethrough_id;
					break;
				default:
					return;
			}

			/* TODO: Catch double "id" characters. ** should not be
			 * detected 
			 */
			while (iter.LineOffset != 0) {
				iter.BackwardChar();
				if (iter.Char == id) {
					start = iter;
					iter.ForwardChar();
					string text = Buffer.GetText (iter, end, false);
					end.ForwardChar();
					Gtk.TextTag tags;
					switch (id) {
						case bold_id:
							tags = (Gtk.TextTag) bold_tag;
							break;
						case italic_id:
							tags = italic_tag;
							break;
						case underlined_id:
							tags = underline_tag;
							break;
						case highlight_id:
							tags = highlight_tag;
							break;
						case strikethrough_id:
							tags = strikethrough_tag;
							break;
						default:
							return;
					}

					Buffer.Delete (ref start, ref end);
					Buffer.InsertWithTags (ref start, text, tags);
					break;
				}
			}
			
		}

	}
}


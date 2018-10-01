import { Component, Input } from '@angular/core';
import { CustomersApiService } from '../services/customers-api.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})

export class NotesComponent {

  @Input() entityId: string;

  notes: Note[];

  statuses: string[];

  settings = {
    add: {
      confirmCreate: true,
    },
    edit: {
      confirmSave: true,
    },
    delete: {
      confirmDelete: true,
    },

    columns: {
      noteId: {
        title: 'noteId',
        editable: false
      },
      text: {
        title: 'text'
      }
    }
  };

  constructor(private cservice: CustomersApiService) { }
  
  ngOnChanges() {
    console.log(this.entityId);
    this
      .cservice
      .getNotes(this.entityId)
      .subscribe((data: Note[]) => {
        this.notes = data;
      });
  }

  onDeleteConfirm(event) {
    console.log(event);
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();

      this
        .cservice
        .deleteNote(this.entityId, event.data.noteId)
        .subscribe();

    } else {
      event.confirm.reject();
    }
  }

  onSaveConfirm(event) {
    console.log(event);
    if (window.confirm('Are you sure you want to save?')) {
      event.newData['name'] += ' + added in code';
      event.confirm.resolve(event.newData);

      this
        .cservice
        .updateNote(this.entityId, event.newData.noteId, event.newData)
        .subscribe();

    } else {
      event.confirm.reject();
    }
  }

  onCreateConfirm(event) {
    console.log(event);
    if (window.confirm('Are you sure you want to create?')) {
      event.newData['name'] += ' + added in code';
      event.newData.entityId = this.entityId;
      event.confirm.resolve(event.newData);
      
      this
        .cservice
        .createNote(event.newData)
        .subscribe();

    } else {
      event.confirm.reject();
    }
  }

}

interface Note {
  noteId: string;
  text: string;
}

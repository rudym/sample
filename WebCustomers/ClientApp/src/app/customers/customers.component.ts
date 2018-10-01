import { Component, OnInit  } from '@angular/core';
import { CustomersApiService } from '../services/customers-api.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})

export class CustomersComponent implements OnInit {
  
  customers: Customer[];

  entityId: string;
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
      customerId: {
        title: 'customerId',
        editable: false
      },
      status: {
        title: 'status',
        valuePrepareFunction: (cell: any, row: any) => {
          return this.statuses[cell];
        },
        editor: {
          type: 'list',
          config: {
            list: [
              { value: 0, title: 'Prospective' },
              { value: 1, title: 'Current' },
              { value: 2, title: 'Non-active' }
            ],
          },
        }
      },
      creationDateTime: {
        title: 'creationDateTime',
        addable: false,
        editable: false,
        valuePrepareFunction: (cell: any, row: any) => {
          let parsedDate = new Date(cell);
          return parsedDate.toLocaleString();
        }
      },
      firstName: {
        title: 'firstName'
      },
      lastName: {
        title: 'lastName'
      },
      mobile: {
        title: 'mobile'
      },
      email: {
        title: 'email'
      }
    }
  };

  constructor(private cservice: CustomersApiService) { }

  ngOnInit() {
    this
      .cservice
      .getCustomers()
      .subscribe((data: Customer[]) => {
        this.customers = data;
      });
    this.statuses = ['Prospective', 'Current', 'Non-active'];
  }

  rowClicked(event) {
    console.log(event);
    this.entityId = event.data.customerId;
  }

  onDeleteConfirm(event) {
    console.log(event);
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();

      this
        .cservice
        .deleteCustomer(event.data.customerId)
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
        .updateCustomer(event.newData.customerId, event.newData)
        .subscribe();

    } else {
      event.confirm.reject();
    }
  }

  onCreateConfirm(event) {
    console.log(event);
    if (window.confirm('Are you sure you want to create?')) {
      event.newData['name'] += ' + added in code';
      event.confirm.resolve(event.newData);
      
      this
        .cservice
        .createCustomer(event.newData)
        .subscribe();

    } else {
      event.confirm.reject();
    }
  }

}

interface Customer {
  customerId: string;
  status: number;
  creationDateTime: string;
  firstName: string;
  lastName: string;
  mobile: string;
  email: string;
}

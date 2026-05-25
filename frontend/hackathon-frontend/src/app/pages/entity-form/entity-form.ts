import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { EntityService } from '../../services/entity.service';

@Component({
  selector: 'app-entity-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './entity-form.html',
  styleUrl: './entity-form.css'
})
export class EntityForm {

  entity = {
    id: 0,
    name: '',
    description: ''
  };

  constructor(
    private entityService: EntityService,
    private router: Router
  ) {}

  addEntity() {

    this.entityService.addEntity(this.entity).subscribe({
      next: (response) => {

        console.log(response);

        alert('Entity Added');

        this.router.navigate(['/entities']);

      },
      error: (error) => {
        console.log(error);
      }
    });

  }

}
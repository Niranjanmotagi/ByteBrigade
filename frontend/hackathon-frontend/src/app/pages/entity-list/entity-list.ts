import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EntityService } from '../../services/entity.service';

@Component({
  selector: 'app-entity-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './entity-list.html',
  styleUrl: './entity-list.css'
})
export class EntityList implements OnInit {

  entities: any[] = [];

  constructor(private entityService: EntityService) {}

  ngOnInit(): void {

    this.loadEntities();

  }

  loadEntities() {

    this.entityService.getEntities().subscribe({
      next: (response: any) => {

        console.log(response);

        this.entities = response.$values;

      },
      error: (error) => {
        console.log(error);
      }
    });

  }

}